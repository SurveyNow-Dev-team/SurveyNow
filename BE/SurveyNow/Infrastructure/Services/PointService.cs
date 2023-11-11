using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Pack;
using Application.DTOs.Response.Point;
using Application.DTOs.Response.Point.History;
using Application.DTOs.Response.Survey;
using Application.DTOs.Response.Transaction;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Services
{
    public class PointService : IPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMomoService _momoService;

        public PointService(IUnitOfWork unitOfWork, IMapper mapper, IMomoService momoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _momoService = momoService;
        }

        public async Task<bool> AddDoSurveyPointAsync(long userId, long surveyId, decimal pointAmount)
        {
            if (userId <= 0 || surveyId <= 0 || pointAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("Id của người dùng, Id khảo sát và số điểm phải lớn hơn 0");
            }
            // Create point history record
            PointHistory pointHistory = new PointHistory()
            {
                UserId = userId,
                SurveyId = surveyId,
                Point = pointAmount,
                PointHistoryType = PointHistoryType.DoSurvey,
                Date = DateTime.UtcNow,
                Description = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.DoSurvey, userId, pointAmount, surveyId),
                Status = TransactionStatus.Success,
            };
            try
            {
                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Add record of point history
                await _unitOfWork.PointHistoryRepository.AddPointHistoryAsync(pointHistory);

                // Add point to user
                await _unitOfWork.UserRepository.UpdateUserPoint(userId, UserPointAction.IncreasePoint, pointAmount);

                // Save changes
                var result = await _unitOfWork.SaveChangeAsync();


                if (result <= 0)
                {
                    throw new Exception("Có lỗi xảy ra khi xử lý phần thưởng điền khảo sát của người dùng");
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new OperationCanceledException($"Có lỗi xảy ra khi xử lý phần thưởng điền khảo sát của người dùng\n{ex.Message}");
            }
        }

        public async Task<MomoPaymentMethodResponse?> CreateMomoPurchasePointOrder(User? user, PointPurchaseRequest purchaseRequest)
        {
            var momoPaymentMethod = await _momoService.CreateMomoPaymentAsync(purchaseRequest);
            if (momoPaymentMethod == null)
            {
                return null;
            }
            return _mapper.Map<MomoPaymentMethodResponse>(momoPaymentMethod);
        }

        public async Task<PagingResponse<ShortPointHistoryResponse>?> GetPaginatedPointHistoryListAsync(long userId, PointHistoryType type, PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest)
        {
            var pageHistories = await _unitOfWork.PointHistoryRepository.GetPointHistoryPaginatedAsync(userId, type, dateFilter, valueFilter, sortOrder, pagingRequest);
            if (pageHistories == null)
            {
                return null;
            }
            PagingResponse<ShortPointHistoryResponse> result = _mapper.Map<PagingResponse<ShortPointHistoryResponse>>(pageHistories);
            return result;
        }

        public async Task<BasePointHistoryResponse?> GetPointHistoryDetailAsync(long id)
        {
            PointHistory? pointHistory = await _unitOfWork.PointHistoryRepository.GetByIdAsync(id);
            if (pointHistory == null)
            {
                return null;
            }
            switch (pointHistory.PointHistoryType)
            {
                case PointHistoryType.PurchasePoint:
                    Transaction? transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                    TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                    PointPurchaseDetailResponse purchaseResult = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);
                    purchaseResult.Transaction = transactionResponse;
                    return purchaseResult;
                case PointHistoryType.RedeemPoint:
                    transaction = await _unitOfWork.TransactionRepository.GetByIdAsync(pointHistory.PointPurchaseId);
                    transactionResponse = _mapper.Map<TransactionResponse>(transaction);
                    PointPurchaseDetailResponse redeemResult = _mapper.Map<PointPurchaseDetailResponse>(pointHistory);
                    redeemResult.Transaction = transactionResponse;
                    return redeemResult;
                case PointHistoryType.DoSurvey:
                    Survey? survey = await _unitOfWork.SurveyRepository.GetByIdAsync(pointHistory.SurveyId);
                    ShortSurveyResponse surveyResponse = _mapper.Map<ShortSurveyResponse>(survey);
                    PointDoSurveyDetailResponse surveyResult = _mapper.Map<PointDoSurveyDetailResponse>(pointHistory);
                    surveyResult.Survey = surveyResponse;
                    return surveyResult;
                case PointHistoryType.PackPurchase:
                    PackPurchase? packPurchase = await _unitOfWork.PackPurchaseRepository.GetByIdAsync(pointHistory.PackPurchaseId);
                    PackPurchaseResponse packPurchaseResponse = _mapper.Map<PackPurchaseResponse>(packPurchase);
                    PointPackPurchaseDetailResponse packResult = _mapper.Map<PointPackPurchaseDetailResponse>(pointHistory);
                    packResult.PackPurchase = packPurchaseResponse;
                    return packResult;
                case PointHistoryType.RefundPoint:
                    return _mapper.Map<BasePointHistoryResponse>(pointHistory);
                default:
                    // Refund point, Gift point and Receiving Point
                    // will be added later on
                    return null;
            }
        }

        public async Task<PointCreateRedeemOrderResponse> ProcessCreateGiftRedeemOrderAsync(PointRedeemRequest redeemRequest)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(redeemRequest.UserId);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy thông tin của người dùng");
            }
            if (user.Point < redeemRequest.PointAmount)
            {
                throw new BadRequestException($"Người dùng không có đủ số điểm. Số dư hiện tại: {user.Point}");
            }
            // Check for any existing pending redeem transaction
            var pendingOrder = await _unitOfWork.TransactionRepository.CheckExistPendingRedeemOrderAsync(user.Id);
            if (pendingOrder)
            {
                throw new ConflictException($"Người dùng có yêu cầu đổi quà đã tạo. Vui lòng chờ cho đến khi yêu cầu trước được xử lý");
            }
            switch (redeemRequest.PaymentMethod)
            {
                case PaymentMethod.Momo:
                    (bool result, PointCreateRedeemOrderResponse? resultData) = await ProcessMomoCreateGiftRedeemOrder(user, redeemRequest);
                    if (!result)
                    {
                        return new PointCreateRedeemOrderResponse()
                        {
                            Status = TransactionStatus.Fail.ToString(),
                            Message = "Không thể tạo yêu cầu đổi quà của người dùng",
                            PointAmount = redeemRequest.PointAmount,
                            MoneyAmount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice,
                            PaymentMethod = redeemRequest.PaymentMethod.ToString()
                        };
                    }
                    return resultData!;
                case PaymentMethod.VnPay:
                    (result, resultData) = await ProcessVnPayCreateGiftRedeemOrder(user, redeemRequest);
                    if (!result)
                    {
                        return new PointCreateRedeemOrderResponse()
                        {
                            Status = TransactionStatus.Fail.ToString(),
                            Message = "Không thể tạo yêu cầu đổi quà của người dùng",
                            PointAmount = redeemRequest.PointAmount,
                            MoneyAmount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice,
                            PaymentMethod = redeemRequest.PaymentMethod.ToString()
                        };
                    }
                    return resultData!;
                default:
                    throw new BadRequestException("Phương thức thanh toán không đươc hỗ trợ");
            }
        }

        private async Task<(bool, PointCreateRedeemOrderResponse?)> ProcessMomoCreateGiftRedeemOrder(User user, PointRedeemRequest redeemRequest)
        {
            try
            {
                // Create new transaction
                Transaction redeemTransaction = new Transaction()
                {
                    UserId = user.Id,
                    TransactionType = TransactionType.RedeemGift,
                    PaymentMethod = PaymentMethod.Momo,
                    Point = redeemRequest.PointAmount,
                    Amount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice,
                    Currency = Currency.VND.ToString(),
                    Date = DateTime.UtcNow,
                    SourceAccount = BusinessData.SurveyNowVnPayAccount,
                    DestinationAccount = redeemRequest.MomoAccount,
                    PurchaseCode = null,
                    Status = TransactionStatus.Pending,
                };

                // Create point history
                var pointHistory = CreatePointHistoryEntity(user, PointHistoryType.RedeemPoint);
                pointHistory!.Point = redeemRequest.PointAmount;
                pointHistory!.Description = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.RedeemPoint, user.Id, redeemRequest.PointAmount, paymentMethod: redeemRequest.PaymentMethod);

                // Add data
                await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.TransactionRepository.AddAsyncReturnEntity(redeemTransaction);
                await _unitOfWork.SaveChangeAsync();

                pointHistory.PointPurchaseId = entity.Id;
                var pointHistoryEntity = await _unitOfWork.PointHistoryRepository.AddAsyncReturnEntity(pointHistory);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.UserRepository.UpdateUserPoint(user.Id, UserPointAction.DecreasePoint, redeemRequest.PointAmount);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitAsync();
                return (true, new PointCreateRedeemOrderResponse()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Yêu cầu đổi quà được tạo thành công. Người dùng sẽ nhận được quà sau khi yêu cầu được xử lý",
                    PointAmount = entity.Point,
                    MoneyAmount = entity.Amount,
                    TransactionId = entity.Id.ToString(),
                    PaymentMethod = entity.PaymentMethod.ToString(),
                    PointHistoryId = pointHistoryEntity.Id.ToString(),
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình tạo yêu cầu đổi quà");
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        public async Task<PointPurchaseResultResponse> ProcessMomoPaymentResultAsync(long userId, MomoCreatePaymentResultRequest resultRequest)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"Không tìm thấy người dùng với Id: {userId}");
            }

            (bool checkTransaction, string message) = _momoService.ValidateMomoPaymentResult(resultRequest);
            // Process failed/cancelled transaction
            if (!checkTransaction)
            {
                return new PointPurchaseResultResponse()
                {
                    Status = TransactionStatus.Fail.ToString(),
                    Message = message,
                    MoneyAmount = resultRequest.amount,
                    PointAmount = resultRequest.amount / BusinessData.BasePointVNDPrice,
                    PaymentMethod = PaymentMethod.Momo.ToString(),
                };
            }
            // Process success transaction
            // Transaction
            Transaction transaction = CreateMomoTransactionEntity(user, resultRequest);
            // Point history
            PointHistory? pointHistory = CreatePointHistoryEntity(user, PointHistoryType.PurchasePoint, resultRequest: resultRequest);
            if (pointHistory == null)
            {
                throw new ArgumentNullException($"Không thể lưu dữ liệu biến động điểm của người dùng");
            }
            try
            {
                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Add transaction
                var transactionEntity = await _unitOfWork.TransactionRepository.AddAsyncReturnEntity(transaction);
                await _unitOfWork.SaveChangeAsync();

                // Update and add point history
                pointHistory.PointPurchaseId = transactionEntity.Id;
                await _unitOfWork.PointHistoryRepository.AddAsync(pointHistory);
                await _unitOfWork.SaveChangeAsync();

                // Update user point amount
                await _unitOfWork.UserRepository.UpdateUserPoint(user.Id, UserPointAction.IncreasePoint, pointHistory.Point);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitAsync();

                return new PointPurchaseResultResponse()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = message,
                    MoneyAmount = transactionEntity.Amount,
                    PointAmount = transactionEntity.Point,
                    PaymentMethod = PaymentMethod.Momo.ToString(),
                    TransactionId = transactionEntity.Id.ToString(),
                    EWalletTransactionId = transactionEntity.PurchaseCode,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new OperationCanceledException($"Có lỗi xảy ra trong quá trình xử lý giao dịch mua điểm của người dùng\n{ex.Message}");
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        private Transaction CreateMomoTransactionEntity(User user, MomoCreatePaymentResultRequest resultRequest)
        {
            return new Transaction()
            {
                UserId = user.Id,
                TransactionType = TransactionType.PurchasePoint,
                PaymentMethod = PaymentMethod.Momo,
                Point = resultRequest.amount / BusinessData.BasePointVNDPrice,
                Amount = resultRequest.amount,
                Currency = Currency.VND.ToString(),
                Date = DateTime.UtcNow,
                SourceAccount = null,
                DestinationAccount = null,
                PurchaseCode = resultRequest.transId,
                Status = TransactionStatus.Success,
            };
        }

        private PointHistory? CreatePointHistoryEntity(User user, PointHistoryType pointHistoryType, MomoCreatePaymentResultRequest resultRequest = null)
        {
            PointHistory result = new PointHistory()
            {
                UserId = user.Id,
                Date = DateTime.UtcNow,
            };
            switch (pointHistoryType)
            {
                case PointHistoryType.PurchasePoint:
                    result.Description = resultRequest.orderInfo;
                    result.PointHistoryType = pointHistoryType;
                    result.Point = resultRequest.amount / BusinessData.BasePointVNDPrice;
                    result.Status = TransactionStatus.Success;
                    return result;
                case PointHistoryType.RefundPoint:
                    result.PointHistoryType = pointHistoryType;
                    result.Status = TransactionStatus.Success;
                    return result;
                case PointHistoryType.RedeemPoint:
                    result.PointHistoryType = pointHistoryType;
                    result.Status = TransactionStatus.Pending;
                    return result;
                default:
                    return null;
            }
        }

        public async Task<bool> RefundPointForUser(long userId, decimal pointAmount, string message)
        {
            // Get user
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"Không tìm thấy người dùng với Id: {userId}");
            }
            try
            {
                // create point history
                var pointHistory = CreatePointHistoryEntity(user, PointHistoryType.RefundPoint);
                var description = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.RefundPoint, userId, pointAmount, refundReason: message);
                pointHistory!.Description = description;
                pointHistory!.Point = pointAmount;

                // refund point to user
                await _unitOfWork.PointHistoryRepository.AddAsync(pointHistory);
                await _unitOfWork.UserRepository.UpdateUserPoint(userId, UserPointAction.IncreasePoint, pointAmount);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình hoàn điểm cho người dùng", ex);
            }
        }

        public async Task<decimal> GetSurveyRewardPointAmount(long surveyId)
        {
            var survey = await _unitOfWork.SurveyRepository.GetByIdAsync(surveyId);
            if (survey == null)
            {
                throw new NotFoundException($"Không tìm thấy khảo sát với Id: {surveyId}");
            }
            if (survey.PackType == null)
            {
                throw new BadRequestException("Khảo sát chưa được mua gói");
            }
            switch (survey.PackType)
            {
                case PackType.Basic:
                    return 0.5m;
                case PackType.Medium:
                    return 0.7m;
                case PackType.Advanced:
                    return 1m;
                case PackType.Expert:
                    return 50m;
                default:
                    throw new BadRequestException("Loại gói không hợp lệ");
            }
        }

        public async Task<PointPurchaseTransactionCreateResponse> CreatePointPurchaseRequest(User user, PointPurchaseTransactionCreateRequest purchaseRequest)
        {
            ValidatePointPurchaseRequest(purchaseRequest);
            var transaction = GeneratePointPurchaseRequestTransactionEntity(user, purchaseRequest);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var transactionEntity = await _unitOfWork.TransactionRepository.AddAsyncReturnEntity(transaction);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                return new PointPurchaseTransactionCreateResponse()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Người dùng tạo yêu cầu nạp điểm thành công. Vui lòng chuyển tiền đến tài khoản của SurveyNow theo thông tin được cung cấp.",
                    PointAmount = transaction.Point,
                    MoneyAmount = transaction.Amount,
                    Currency = transaction.Currency,
                    PaymentMethod = transaction.PaymentMethod.ToString(),
                    DestinationAccount = transaction.DestinationAccount!,
                    Description = $"SurveyNow - Người dùng với mã số: {user.Id} - Nạp {transaction.Point} điểm vào tài khoản - Với mã giao dịch yêu cầu: {transaction.Id}",
                    TransactionId = transactionEntity.Id,

                };
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình tạo yêu cầu nạp điểm");
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        private Transaction GeneratePointPurchaseRequestTransactionEntity(User user, PointPurchaseTransactionCreateRequest purchaseRequest)
        {
            Transaction transaction = new Transaction()
            {
                UserId = user.Id,
                TransactionType = TransactionType.PurchasePoint,
                Point = purchaseRequest.PointAmount,
                Amount = purchaseRequest.PointAmount * BusinessData.BasePointVNDPrice,
                Currency = Currency.VND.ToString(),
                Date = DateTime.UtcNow,
                SourceAccount = null,
                PurchaseCode = null,
                Status = TransactionStatus.Pending,
            };

            switch (purchaseRequest.PaymentMethod)
            {
                case PaymentMethod.Momo:
                    transaction.PaymentMethod = PaymentMethod.Momo;
                    transaction.DestinationAccount = BusinessData.SurveyNowMomoAccount;
                    break;
                case PaymentMethod.VnPay:
                    transaction.PaymentMethod = PaymentMethod.VnPay;
                    transaction.DestinationAccount = BusinessData.SurveyNowVnPayAccount;
                    break;
                default:
                    throw new BadRequestException("Phương thức thanh toán nạp điểm không hợp lệ");
            }
            return transaction;
        }

        private void ValidatePointPurchaseRequest(PointPurchaseTransactionCreateRequest purchaseRequest)
        {
            if (purchaseRequest == null) throw new BadRequestException("Yêu cầu đổi điểm không hợp lệ. Vui lòng nhập lại yêu cầu");
            if (purchaseRequest.PointAmount <= 0) throw new BadRequestException("Số điểm cần nạp phải lớn hơn 0");
        }

        private async Task<(bool, PointCreateRedeemOrderResponse?)> ProcessVnPayCreateGiftRedeemOrder(User user, PointRedeemRequest redeemRequest)
        {
            try
            {
                // Create new transaction
                Transaction redeemTransaction = new Transaction()
                {
                    UserId = user.Id,
                    TransactionType = TransactionType.RedeemGift,
                    PaymentMethod = PaymentMethod.VnPay,
                    Point = redeemRequest.PointAmount,
                    Amount = redeemRequest.PointAmount * BusinessData.BasePointVNDPrice,
                    Currency = Currency.VND.ToString(),
                    Date = DateTime.UtcNow,
                    SourceAccount = BusinessData.SurveyNowVnPayAccount,
                    DestinationAccount = redeemRequest.MomoAccount,
                    PurchaseCode = null,
                    Status = TransactionStatus.Pending,
                };

                // Create point history
                var pointHistory = CreatePointHistoryEntity(user, PointHistoryType.RedeemPoint);
                pointHistory!.Point = redeemRequest.PointAmount;
                pointHistory!.Description = EnumUtil.GeneratePointHistoryDescription(PointHistoryType.RedeemPoint, user.Id, redeemRequest.PointAmount, paymentMethod: redeemRequest.PaymentMethod);

                // Add data
                await _unitOfWork.BeginTransactionAsync();
                var entity = await _unitOfWork.TransactionRepository.AddAsyncReturnEntity(redeemTransaction);
                await _unitOfWork.SaveChangeAsync();

                pointHistory.PointPurchaseId = entity.Id;
                var pointHistoryEntity = await _unitOfWork.PointHistoryRepository.AddAsyncReturnEntity(pointHistory);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.UserRepository.UpdateUserPoint(user.Id, UserPointAction.DecreasePoint, redeemRequest.PointAmount);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitAsync();
                return (true, new PointCreateRedeemOrderResponse()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Yêu cầu đổi quà được tạo thành công. Người dùng sẽ nhận được quà sau khi yêu cầu được xử lý",
                    PointAmount = entity.Point,
                    MoneyAmount = entity.Amount,
                    TransactionId = entity.Id.ToString(),
                    PaymentMethod = entity.PaymentMethod.ToString(),
                    PointHistoryId = pointHistoryEntity.Id.ToString(),
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình tạo yêu cầu đổi quà");
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }
    }
}

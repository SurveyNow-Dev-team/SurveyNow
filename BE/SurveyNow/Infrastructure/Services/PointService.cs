using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Momo;
using Application.DTOs.Request.Point;
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
                throw new ArgumentOutOfRangeException("Paramater(s) is out of range. All parameters range must be larger than 0");
            }
            try
            {
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
                    throw new Exception("Failed add do survey point transaction");
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new OperationCanceledException($"Failed to add point for user survey completion\n{ex.Message}");
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
                default:
                    // Refund point, Gift point and Receiving Point
                    // will be added later on
                    return null;
            }
        }

        public async Task<PointPurchaseResultResponse> ProcessMomoPaymentResultAsync(long userId, MomoCreatePaymentResultRequest resultRequest)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if(user == null)
            {
                throw new NotFoundException("Cannot find user information");
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
            try
            {
                // Transaction
                Transaction transaction = CreateMomoTransactionEntity(user, resultRequest);
                // Point history
                PointHistory? pointHistory = CreatePointHistoryEntity(user, PointHistoryType.PurchasePoint, resultRequest: resultRequest);
                if(pointHistory == null)
                {
                    throw new ArgumentNullException($"Cannot create point history data");
                }

                // Begin transaction
                await _unitOfWork.BeginTransactionAsync();

                // Add transaction
                var transactionEntity =  await _unitOfWork.TransactionRepository.AddAsyncReturnEntity(transaction);
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
                throw new OperationCanceledException($"Failed to process user point purchase transaction\n{ex.Message}");
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
                Status = TransactionStatus.Success,
            };
            switch (pointHistoryType)
            {
                case PointHistoryType.PurchasePoint:
                    result.Description = resultRequest.orderInfo;
                    result.PointHistoryType = pointHistoryType;
                    result.Point = resultRequest.amount / BusinessData.BasePointVNDPrice;
                    return result;
                default:
                    return null;
            }
        }
    }
}

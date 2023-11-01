using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System.Text.RegularExpressions;

namespace Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPointService _pointService;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IPointService pointService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _pointService = pointService;
        }

        public async Task<ProccessRedeemTransactionResult> CancelRedeemTransaction(long id)
        {
            var redeemTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (redeemTransaction == null)
            {
                throw new NotFoundException($"Không tìm thấy thông tin của giao dịch với ID: {id}");
            }
            if (redeemTransaction.TransactionType != TransactionType.RedeemGift)
            {
                throw new BadRequestException($"Giao dịch với mã số {id} không phải là giao dịch đổi quà");
            }
            if (redeemTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException($"Giao dịch không ở trạng thái chờ xử lý");
            }
            var redeemPointHistory = await _unitOfWork.PointHistoryRepository.GetByTransactionId(redeemTransaction.Id);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                redeemTransaction.Status = TransactionStatus.Cancel;
                _unitOfWork.TransactionRepository.Update(redeemTransaction);

                redeemPointHistory!.Status = TransactionStatus.Cancel;
                _unitOfWork.PointHistoryRepository.Update(redeemPointHistory);

                await _unitOfWork.SaveChangeAsync();

                var refundResult = await _pointService.RefundPointForUser(redeemTransaction.UserId, redeemTransaction.Point, $"Hoàn điểm cho yêu cầu đổi quà với Id: {redeemTransaction.Id}");

                await _unitOfWork.CommitAsync();
                return new ProccessRedeemTransactionResult()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Thành công hủy giao dịch đổi quà và hoàn điểm lại cho người dùng",
                    TransactionId = redeemTransaction.Id,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình hủy giao dịch đổi quà của người dùng", ex);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        public async Task<PagingResponse<TransactionResponse>> GetPaginatedPendingRedeemTransactionsAsync(PagingRequest pagingRequest)
        {
            if (pagingRequest.Page < 1 || pagingRequest.RecordsPerPage < 1 || pagingRequest.Page == null || pagingRequest.RecordsPerPage == null)
            {
                throw new BadRequestException("Tiêu chí phân trang cho kết quả không hợp lệ");
            }
            try
            {
                var entityList = await _unitOfWork.TransactionRepository.GetPendingRedeemTransactionList(pagingRequest);
                var result = _mapper.Map<PagingResponse<TransactionResponse>>(entityList);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi xảy ra trong quá trình tìm kiếm dữ liệu các giao dịch đổi quà đang chờ xử lý", ex);
            }
        }

        public async Task<TransactionResponse> GetTransactionsAsync(long id)
        {

            var entity = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException($"Không tìm thấy dữ liệu của giao dịch với Id: {id}");
            }
            var result = _mapper.Map<TransactionResponse>(entity);
            return result;
        }

        public async Task<ProccessRedeemTransactionResult> ProcessRedeemTransaction(long id, UpdatePointRedeemTransactionRequest request)
        {
            var redeemTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (redeemTransaction == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu của yêu cầu đổi quà");
            }
            if (redeemTransaction.TransactionType != TransactionType.RedeemGift)
            {
                throw new BadRequestException($"Giao dịch với mã số {id} không phải là giao dịch đổi quà");
            }
            if (redeemTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException("Chỉ có thể xử lý giao địch đang được chờ");
            }
            // Need to validate data in request (eWallet transaction Id,...)
            var redeemPointHistory = await _unitOfWork.PointHistoryRepository.GetByTransactionId(redeemTransaction.Id);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                redeemTransaction.PurchaseCode = request.MomoTransactionId.Trim();
                redeemTransaction.Status = TransactionStatus.Success;
                _unitOfWork.TransactionRepository.Update(redeemTransaction);

                redeemPointHistory!.Status = TransactionStatus.Success;
                _unitOfWork.PointHistoryRepository.Update(redeemPointHistory);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitAsync();
                return new ProccessRedeemTransactionResult()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Yêu cầu đổi quà của người dùng được xử lý thành công",
                    TransactionId = redeemTransaction.Id,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xày ra trong quá trình xử lý yêu cầu đổi quà của người dùng", ex);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        public async Task<PagingResponse<TransactionResponse>> GetTransactionHistory(PagingRequest pagingRequest, TransactionHistoryRequest historyRequest)
        {
            if (pagingRequest.Page < 1 || pagingRequest.RecordsPerPage < 1 || pagingRequest.Page == null || pagingRequest.RecordsPerPage == null)
            {
                throw new BadRequestException("Tiêu chí phân trang cho kết quả không hợp lệ");
            }
            var transactionList = await _unitOfWork.TransactionRepository.GetTransactionHistory(pagingRequest, historyRequest);
            return _mapper.Map<PagingResponse<TransactionResponse>>(transactionList);
        }

        public async Task<PagingResponse<TransactionResponse>> GetPaginatedPendingPurchaseTransactionsAsync(long? id, PagingRequest pagingRequest)
        {
            if (pagingRequest.Page < 1 || pagingRequest.RecordsPerPage < 1 || pagingRequest.Page == null || pagingRequest.RecordsPerPage == null)
            {
                throw new BadRequestException("Tiêu chí phân trang cho kết quả không hợp lệ");
            }
            var transactionList = await _unitOfWork.TransactionRepository.GetPendingPurchaseTransactionList(id, pagingRequest);
            return _mapper.Map<PagingResponse<TransactionResponse>>(transactionList);
        }

        public async Task<ProccessRedeemTransactionResult> CancelPurchaseTransaction(long id)
        {
            var purchaseTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (purchaseTransaction == null)
            {
                throw new NotFoundException($"Không tìm thấy thông tin của giao dịch với ID: {id}");
            }
            if (purchaseTransaction.TransactionType != TransactionType.PurchasePoint)
            {
                throw new BadRequestException($"Giao dịch với mã số {id} không phải là giao dịch nạp điểm");
            }
            if (purchaseTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException($"Giao dịch không ở trạng thái chờ xử lý");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                purchaseTransaction.Status = TransactionStatus.Cancel;
                _unitOfWork.TransactionRepository.Update(purchaseTransaction);

                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                return new ProccessRedeemTransactionResult()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = $"Thành công hủy giao dịch nạp điểm với mã số {id} của người dùng",
                    TransactionId = purchaseTransaction.Id,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xảy ra trong quá trình hủy giao dịch nạp điểm của người dùng", ex);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        public async Task<ProccessRedeemTransactionResult> ProcessPurchaseTransaction(long id, UpdatePointPurchaseTransactionRequest request)
        {
            var purchaseTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (purchaseTransaction == null)
            {
                throw new NotFoundException("Không tìm thấy dữ liệu của yêu cầu nạp điểm");
            }
            if (purchaseTransaction.TransactionType != TransactionType.PurchasePoint)
            {
                throw new BadRequestException($"Giao dịch với mã số {id} không phải là giao dịch nạp điểm");
            }
            if (purchaseTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException("Chỉ có thể xử lý giao địch đang được chờ");
            }
            // Need to validate data in request (eWallet transaction Id,...)
            ValidatePointPurchaseRequest(purchaseTransaction, request);
            purchaseTransaction.Status = TransactionStatus.Success;
            purchaseTransaction.SourceAccount = request.SourceAccount;
            purchaseTransaction.PurchaseCode = request.EWalletTransactionId;
            var pointHistory = GeneratePointPurchaseHistoryEntity(purchaseTransaction);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _unitOfWork.TransactionRepository.Update(purchaseTransaction);
                await _unitOfWork.PointHistoryRepository.AddAsync(pointHistory);
                await _unitOfWork.UserRepository.UpdateUserPoint(purchaseTransaction.UserId, UserPointAction.IncreasePoint, purchaseTransaction.Point);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitAsync();
                return new ProccessRedeemTransactionResult()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Yêu cầu nạp điểm của người dùng được xử lý thành công. Số điểm trong tài khoản của người dùng đã được cập nhật",
                    TransactionId = purchaseTransaction.Id,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Có lỗi xày ra trong quá trình xử lý yêu cầu nạp điểm của người dùng", ex);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        private PointHistory GeneratePointPurchaseHistoryEntity(Transaction transaction)
        {
            PointHistory pointHistory = new PointHistory()
            {
                Date = DateTime.UtcNow,
                Description = $"Người dùng nạp điểm vào tài khoản - Số lượng điểm: {transaction.Point} - Số tiền: {transaction.Amount} - Phương thức thanh toán: {transaction.PaymentMethod.ToString()} - Cách thức: Thủ công",
                PointHistoryType = PointHistoryType.PurchasePoint,
                Point = transaction.Point,
                Status = TransactionStatus.Success,
                UserId = transaction.UserId,
                PointPurchaseId = transaction.Id,
                PackPurchaseId = null,
                SurveyId = null,
            };
            return pointHistory;
        }

        private void ValidatePointPurchaseRequest(Transaction transaction, UpdatePointPurchaseTransactionRequest request)
        {
            // Validate phone number
            string phoneNumberPattern = @"\b0\d{9}\b";
            if (!Regex.IsMatch(request.SourceAccount, phoneNumberPattern))
            {
                throw new BadRequestException("Số tài khoản (số điện thoại) của ví điện tử của người dùng không hợp lệ");
            }
            // Validate eWallet transaction Id
            if (transaction.PaymentMethod == PaymentMethod.Momo)
            {
                string momoTransactionIdPattern = @"\b\d{11}\b";
                if (!Regex.IsMatch(request.EWalletTransactionId, momoTransactionIdPattern))
                {
                    throw new BadRequestException("Mã giao dịch momo không hợp lệ");
                }
            }
            else if (transaction.PaymentMethod == PaymentMethod.VnPay)
            {

            }
        }
    }
}

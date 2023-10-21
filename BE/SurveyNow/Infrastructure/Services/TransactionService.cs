using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Enums;

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

        public async Task<PagingResponse<TransactionResponse>> GetPaginatedPendingTransactionsAsync(PagingRequest pagingRequest)
        {
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
                throw new NotFoundException("Không tìm thấy dữ liệu của yêu cầu đổi điểm");
            }
            if (redeemTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException("Chỉ có thể xử lý giao địch đang được chờ");
            }
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
            if(pagingRequest.Page < 1 || pagingRequest.RecordsPerPage < 1 || pagingRequest.Page == null || pagingRequest.RecordsPerPage == null)
            {
                throw new BadRequestException("Tiêu chí phân trang cho kết quả không hợp lệ");
            }
            var transactionList = await _unitOfWork.TransactionRepository.GetTransactionHistory(pagingRequest, historyRequest);
            return _mapper.Map<PagingResponse<TransactionResponse>>(transactionList);
        }
    }
}

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
                throw new NotFoundException("Cannot find redeem transaction data");
            }
            if (redeemTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException("Can only cancel pending transaction");
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

                var refundResult = await _pointService.RefundPointForUser(redeemTransaction.UserId, redeemTransaction.Point, $"Refund point for point redeem transaction: {redeemTransaction.Id}");

                await _unitOfWork.CommitAsync();
                return new ProccessRedeemTransactionResult()
                {
                    Status = TransactionStatus.Success.ToString(),
                    Message = "Successfully cancel point redeem transaction and refund point for user",
                    TransactionId = redeemTransaction.Id,
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Error when trying to cancel redeem transaction", ex);
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
                throw new Exception("Unexpected error occurred when trying to retrieve pending point redeem transaction list", ex);
            }
        }

        public async Task<TransactionResponse> GetPendingTransactionsAsync(long id)
        {

            var entity = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new NotFoundException("Cannot find transaction with the given id");
            }
            var result = _mapper.Map<TransactionResponse>(entity);
            return result;
        }

        public async Task<ProccessRedeemTransactionResult> ProcessRedeemTransaction(long id, UpdatePointRedeemTransactionRequest request)
        {
            var redeemTransaction = await _unitOfWork.TransactionRepository.GetByIdAsync(id);
            if (redeemTransaction == null)
            {
                throw new NotFoundException("Cannot find redeem transaction data");
            }
            if(redeemTransaction.Status != TransactionStatus.Pending)
            {
                throw new BadRequestException("Can only process pending transaction");
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
                    Message = "Successfully process user's point redeem transaction",
                    TransactionId = redeemTransaction.Id,
                };

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Error when trying to process redeem transaction", ex);
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }
    }
}

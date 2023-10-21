using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;

namespace Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<PagingResponse<TransactionResponse>> GetPaginatedPendingTransactionsAsync(PagingRequest pagingRequest);
        Task<TransactionResponse> GetTransactionsAsync(long id);
        Task<ProccessRedeemTransactionResult> CancelRedeemTransaction(long id);
        Task<ProccessRedeemTransactionResult> ProcessRedeemTransaction(long id, UpdatePointRedeemTransactionRequest request);
        Task<PagingResponse<TransactionResponse>> GetTransactionHistory(PagingRequest pagingRequest, TransactionHistoryRequest historyRequest);
    }
}

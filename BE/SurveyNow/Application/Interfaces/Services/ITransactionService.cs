using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<PagingResponse<TransactionResponse>> GetPaginatedPendingRedeemTransactionsAsync(PagingRequest pagingRequest);
        Task<TransactionResponse> GetTransactionsAsync(long id);
        Task<ProccessRedeemTransactionResult> CancelRedeemTransaction(long id);
        Task<ProccessRedeemTransactionResult> ProcessRedeemTransaction(long id, UpdatePointRedeemTransactionRequest request);
        Task<PagingResponse<TransactionResponse>> GetTransactionHistory(PagingRequest pagingRequest, TransactionHistoryRequest historyRequest);
        Task<PagingResponse<TransactionResponse>> GetPaginatedPendingPurchaseTransactionsAsync(long? id, PagingRequest pagingRequest);
        Task<ProccessRedeemTransactionResult> CancelPurchaseTransaction(long id);
        Task<ProccessRedeemTransactionResult> ProcessPurchaseTransaction(long id, UpdatePointPurchaseTransactionRequest request);
        Task<PagingResponse<TransactionResponse>> GetUserTransactions(UserTransactionRequest request, PagingRequest pagingRequest, User user);
    }
}

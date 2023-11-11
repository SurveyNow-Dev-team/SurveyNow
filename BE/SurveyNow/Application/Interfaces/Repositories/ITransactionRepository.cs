using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.DTOs.Response.Transaction;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    Task<bool> CheckExistPendingRedeemOrderAsync(long userId);
    Task<PagingResponse<Transaction>> GetPendingRedeemTransactionList(PagingRequest pagingRequest);
    Task<PagingResponse<Transaction>> GetTransactionHistory(PagingRequest pagingRequest, TransactionHistoryRequest historyRequest);
    Task<PagingResponse<Transaction>> GetPendingPurchaseTransactionList(long? id, PagingRequest pagingRequest);
    Task<PagingResponse<Transaction>> GetUserTransactions(UserTransactionRequest request, PagingRequest pagingRequest, long userId);
}
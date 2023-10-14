using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ITransactionRepository: IBaseRepository<Transaction>
{
    Task<bool> CheckExistPendingRedeemOrderAsync();
    Task<PagingResponse<Transaction>> GetPendingRedeemTransactionList(PagingRequest pagingRequest);
}
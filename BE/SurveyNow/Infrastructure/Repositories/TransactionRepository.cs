using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(AppDbContext context, ILogger<BaseRepository<Transaction>> logger) : base(context, logger)
    {
    }

    public async Task<bool> CheckExistPendingRedeemOrderAsync()
    {
        return await _dbSet.AnyAsync(t => t.TransactionType == Domain.Enums.TransactionType.RedeemGift && t.Status == Domain.Enums.TransactionStatus.Pending);
    }

    public async Task<PagingResponse<Transaction>> GetPendingRedeemTransactionList(PagingRequest pagingRequest)
    {
        // Expression list
        Expression<Func<Transaction, bool>> pendingRedeemTransaction = (t => t.TransactionType == TransactionType.RedeemGift && t.Status == TransactionStatus.Pending);

        // Sorting order
        Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> sortOrder = (q => q.OrderBy(t => t.Date));

        var filterList = await GetPaginateAsync(pendingRedeemTransaction, sortOrder, "", page: pagingRequest.Page, size: pagingRequest.RecordsPerPage);

        return filterList;
    }
}
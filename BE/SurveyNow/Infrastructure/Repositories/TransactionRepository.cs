using Application.DTOs.Request;
using Application.DTOs.Request.Transaction;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Utils;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

    public async Task<PagingResponse<Transaction>> GetTransactionHistory(PagingRequest pagingRequest, TransactionHistoryRequest historyRequest)
    {
        var durationFilter = GetDurationFilter(historyRequest);
        var statusFilter = GetStatusFilter(historyRequest);
        var transactionTypeFilter = GetTransactionTypeFilter(historyRequest);
        var combinedFilter = GetCombinedFilterExpression(durationFilter, statusFilter, transactionTypeFilter);
        var sortQuery = GetSortOrderQuery(historyRequest);
        return await GetPaginateAsync(combinedFilter, sortQuery, "", pagingRequest.Page, pagingRequest.RecordsPerPage);
    }

    private Expression<Func<Transaction, bool>>? GetDurationFilter(TransactionHistoryRequest historyRequest)
    {
        if (historyRequest.FromDate == null || historyRequest.ToDate == null)
        {
            return null;
        }
        var fromDate = DateUtil.ConvertStringToDateTimeV1(historyRequest.FromDate);
        var toDate = DateUtil.ConvertStringToDateTimeV1(historyRequest.ToDate);
        if (fromDate == null || toDate == null)
        {
            return null;
        }
        if (DateTime.Compare((DateTime)fromDate!, (DateTime)toDate!) > 0)
        {
            (fromDate, toDate) = (toDate, fromDate);
        }
        return (t => t.Date >= fromDate && t.Date <= toDate);
    }

    private Expression<Func<Transaction, bool>>? GetStatusFilter(TransactionHistoryRequest historyRequest)
    {
        if (historyRequest.Status == null)
        {
            return null;
        }
        return (t => t.Status == historyRequest.Status);
    }
    private Expression<Func<Transaction, bool>>? GetTransactionTypeFilter(TransactionHistoryRequest historyRequest)
    {
        if (historyRequest.TransactionType == null)
        {
            return null;
        }
        return (t => t.TransactionType == historyRequest.TransactionType);
    }

    private Expression<Func<Transaction, bool>>? GetCombinedFilterExpression(Expression<Func<Transaction, bool>>? durationFilter,
        Expression<Func<Transaction, bool>>? statusFilter,
        Expression<Func<Transaction, bool>>? transactionTypeFilter)
    {
        if (durationFilter == null && statusFilter == null && transactionTypeFilter == null)
        {
            return null;
        }
        Expression combinedExpression = null;
        ParameterExpression parameter = Expression.Parameter(typeof(Transaction));
        if (durationFilter != null)
        {
            combinedExpression = Expression.Invoke(durationFilter, parameter);
        }
        if (statusFilter != null)
        {
            combinedExpression = combinedExpression != null
                ? Expression.AndAlso(combinedExpression, Expression.Invoke(statusFilter, parameter))
                : Expression.Invoke(statusFilter, parameter);
        }
        if (transactionTypeFilter != null)
        {
            combinedExpression = combinedExpression != null
                ? Expression.AndAlso(combinedExpression, Expression.Invoke(transactionTypeFilter, parameter))
                : Expression.Invoke(transactionTypeFilter, parameter);
        }
        return Expression.Lambda<Func<Transaction, bool>>(combinedExpression!, parameter);
    }

    private Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> GetSortOrderQuery(TransactionHistoryRequest historyRequest)
    {
        switch (historyRequest.SortingOrder)
        {
            case TransactionHistorySortingOrder.DateDescending:
                return (query => query.OrderByDescending(t => t.Date));
            case TransactionHistorySortingOrder.DateAscending:
                return (query => query.OrderBy(t => t.Date));
            case TransactionHistorySortingOrder.AmountDescending:
                return (query => query.OrderByDescending(t => t.Amount));
            case TransactionHistorySortingOrder.AmountAscending:
                return (query => query.OrderBy(t => t.Amount));
            default:
                return (query => query.OrderByDescending(t => t.Date));
        }
    }

    public async Task<PagingResponse<Transaction>> GetPendingPurchaseTransactionList(long? id, PagingRequest pagingRequest)
    {
        Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>> sortOrder = (q => q.OrderBy(t => t.Date));
        PagingResponse<Transaction> result = null;
        if(id is null)
        {
            Expression<Func<Transaction, bool>> filterExpression = (t => t.TransactionType == TransactionType.PurchasePoint && t.Status == TransactionStatus.Pending);
            result = await GetPaginateAsync(filterExpression, sortOrder, "", page: pagingRequest.Page, size: pagingRequest.RecordsPerPage);
        }
        else if(id is not null && id.HasValue)
        {
            Expression<Func<Transaction, bool>> filterExpression = (t => t.TransactionType == TransactionType.PurchasePoint && t.Status == TransactionStatus.Pending && t.Id == id);
            result = await GetPaginateAsync(filterExpression, sortOrder, "", page: pagingRequest.Page, size: pagingRequest.RecordsPerPage);
        }
        return result;
    }
}
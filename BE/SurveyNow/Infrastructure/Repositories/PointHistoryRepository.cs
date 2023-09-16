using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PointHistoryRepository : BaseRepository<PointHistory>, IPointHistoryRepository
{
    public PointHistoryRepository(AppDbContext context, ILogger<BaseRepository<PointHistory>> logger) : base(context,
        logger)
    {
    }

    public async Task<PointHistory?> GetPointPurchaseDetailAsync(long id)
    {
        return await _dbSet.Include(p => p.PointPurchase)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PointHistory>?> GetPointPurchasesFilteredAsync(PointDateFilterRequest dateFilter,
        PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest, long userId)
    {
        // Get and combine filter expressions
        Expression<Func<PointHistory, bool>> userIdExp = (p => p.UserId == userId);
        Expression<Func<PointHistory, bool>> typeExp = GetPointHistoryTypeFilterExpression(PointHistoryType.Purchase);
        Expression<Func<PointHistory, bool>>? dateExp = GetDateFilterExpression(dateFilter);
        Expression<Func<PointHistory, bool>>? pointRangeExp = GetPointRangeFilterExpression(valueFilter);

        var parameter = Expression.Parameter(typeof(PointHistory), "p");

        var combinedExp = Expression.AndAlso(
            Expression.Invoke(userIdExp, parameter),
            Expression.Invoke(typeExp, parameter));

        if (dateExp != null)
        {
            combinedExp = Expression.AndAlso(combinedExp, Expression.Invoke(dateExp, parameter));
        }

        if (pointRangeExp != null)
        {
            combinedExp = Expression.AndAlso(combinedExp, Expression.Invoke(pointRangeExp, parameter));
        }

        var combinedExpression = Expression.Lambda<Func<PointHistory, bool>>(combinedExp, parameter);

        // Get sorting expression
        var orderByFunction = GetOrderByFunction(sortOrder);

        // Get filtered and sorted item collection
        var filteredItems = await GetAsync(combinedExpression, orderByFunction);

        // Apply pagination
        if (filteredItems != null && (filteredItems.Count() > 0))
        {
            int page = (int)pagingRequest.Page, pageItemCount = (int)pagingRequest.RecordsPerPage;
            var result = filteredItems
                .Skip((page - 1) * pageItemCount)
                .Take(pageItemCount)
                .ToList();

            return result;
        }

        return null;
    }

    private Expression<Func<PointHistory, bool>>? GetDateFilterExpression(PointDateFilterRequest dateFilter)
    {
        if (dateFilter.IsValid() != 0)
        {
            dateFilter.ChangeValues();
            return (p => p.Date >= dateFilter.GetFromDate() && p.Date <= dateFilter.GetToDate());
        }

        return null;
    }

    private Expression<Func<PointHistory, bool>>? GetPointRangeFilterExpression(PointValueFilterRequest valueFilter)
    {
        if (valueFilter.IsValid() != 0)
        {
            valueFilter.ChangeValues();
            return (p => p.Point >= valueFilter.MinPoint && p.Point <= valueFilter.MaxPoint);
        }

        return null;
    }

    private Expression<Func<PointHistory, bool>> GetPointHistoryTypeFilterExpression(PointHistoryType type)
    {
        return (p => p.PointHistoryType == type);
    }

    private Func<IQueryable<PointHistory>, IOrderedQueryable<PointHistory>> GetOrderByFunction(
        PointSortOrderRequest sortOrder)
    {
        switch (sortOrder.SortingOrder)
        {
            case PointSortingOrder.DateDescending:
                return (q => q.OrderByDescending(p => p.Date));
            case PointSortingOrder.DateAscending:
                return (q => q.OrderBy(p => p.Date));
            case PointSortingOrder.PointDescending:
                return (q => q.OrderByDescending(p => p.Point));
            case PointSortingOrder.PointAscending:
                return (q => q.OrderBy(p => p.Point));
            default:
                return (q => q.OrderByDescending(p => p.Date));
        }
    }

    public async Task<PointHistory?> GetPointRedeemDetailAsync(long id)
    {
        return await _dbSet.Include(p => p.PointPurchase)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
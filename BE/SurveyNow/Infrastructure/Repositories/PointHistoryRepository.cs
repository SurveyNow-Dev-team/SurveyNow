using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Application.DTOs.Response;
using Application.Utils;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories;

public class PointHistoryRepository : BaseRepository<PointHistory>, IPointHistoryRepository
{
    public PointHistoryRepository(AppDbContext context, ILogger<BaseRepository<PointHistory>> logger) : base(context,
        logger)
    {
    }

    public async Task<PagingResponse<PointHistory>?> GetPointHistoryPaginatedAsync(long userId, PointHistoryType type, PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest)
    {
        // Get and combine filter expressions
        Expression<Func<PointHistory, bool>> userIdExp = (p => p.UserId == userId);
        Expression<Func<PointHistory, bool>>? typeExp = GetPointHistoryTypeFilterExpression(type);
        Expression<Func<PointHistory, bool>>? dateExp = GetDateFilterExpression(dateFilter);
        Expression<Func<PointHistory, bool>>? pointRangeExp = GetPointRangeFilterExpression(valueFilter);

        // Declare expression parameter and combine expression body
        ParameterExpression parameter = Expression.Parameter(typeof(PointHistory));
        Expression combinedBody = Expression.Invoke(userIdExp, parameter);

        if (typeExp != null)
        {
            combinedBody = Expression.AndAlso(combinedBody, Expression.Invoke(typeExp, parameter));
        }

        if (dateExp != null)
        {
            combinedBody = Expression.AndAlso(combinedBody, Expression.Invoke(dateExp, parameter));
        }

        if (pointRangeExp != null)
        {
            combinedBody = Expression.AndAlso(combinedBody, Expression.Invoke(pointRangeExp, parameter));
        }

        var combinedExpression = Expression.Lambda<Func<PointHistory, bool>>(combinedBody, parameter);

        // Get sorting expression
        var orderByFunction = GetOrderByFunction(sortOrder);

        // Get filtered and sorted item collection
        var filteredItems = await GetAsync(combinedExpression, orderByFunction);

        // Apply pagination to result
        if (filteredItems != null && (filteredItems.Count() > 0))
        {
            PagingResponse<PointHistory>? result = filteredItems.ToList().Paginate(pagingRequest.Page, pagingRequest.RecordsPerPage);
            return result;
        }
        return null;
    }

    public async Task<PointHistory> AddPointHistoryAsync(PointHistory pointHistory)
    {
        if (pointHistory == null)
        {
            throw new ArgumentNullException("Point History information is invalid");
        }
        try
        {
            EntityEntry<PointHistory> result = await _dbSet.AddAsync(pointHistory);
            return result.Entity;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Adding Point History Task failed.", ex);
        }
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

    private Expression<Func<PointHistory, bool>>? GetPointHistoryTypeFilterExpression(PointHistoryType type)
    {
        if(type == PointHistoryType.None)
        {
            return null;
        }
        else
        {
            return (p => p.PointHistoryType == type);
        }
    }

    private Func<IQueryable<PointHistory>, IOrderedQueryable<PointHistory>> GetOrderByFunction(PointSortOrderRequest sortOrder)
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
}
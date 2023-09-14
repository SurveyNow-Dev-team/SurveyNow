using Application.DTOs.Request.Point;
using Application.DTOs.Request;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPointHistoryRepository: IBaseRepository<PointHistory>
{
    Task<PointHistory?> GetPointPurchaseDetailAsync(long id);
    Task<List<PointHistory>?> GetPointPurchasesFilteredAsync(PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest, long userId);
}
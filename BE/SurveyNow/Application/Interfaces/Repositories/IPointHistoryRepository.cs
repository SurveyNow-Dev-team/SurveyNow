using Application.DTOs.Request.Point;
using Application.DTOs.Request;
using Domain.Entities;
using Application.DTOs.Response;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IPointHistoryRepository: IBaseRepository<PointHistory>
{
    Task<PagingResponse<PointHistory>> GetPointHistoryPaginatedAsync(long userId, PointHistoryType type, PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest);
    Task<PointHistory?> AddPointHistoryAsync(PointHistory pointHistory);
    Task<PointHistory?> GetByTransactionId(long transactionId);
}
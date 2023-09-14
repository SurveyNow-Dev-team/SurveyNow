using Application.DTOs.Request;
using Application.DTOs.Request.Point;
using Application.DTOs.Response;
using Application.DTOs.Response.Point;

namespace Application.Interfaces.Services
{
    public interface IPointService
    {
        #region Purchase
        Task<PointPurchaseDetailResponse> GetPointPurchaseDetailAsync(long id);

        Task<PagingResponse<PointPurchaseResponse>?> GetPointPurchasesFilteredAsync(PointDateFilterRequest dateFilter, PointValueFilterRequest valueFilter, PointSortOrderRequest sortOrder, PagingRequest pagingRequest, long userId);
        #endregion
        #region Redeem
        #endregion
    }
}

using Application.DTOs.Response.Point;
using Application.DTOs.Response.User;

namespace Application.Interfaces.Services
{
    public interface IPointService
    {
        #region Purchase
        Task<PointPurchaseDetailResponse> GetPointPurchaseDetail(long id);
        #endregion
        #region Redeem
        #endregion
    }
}

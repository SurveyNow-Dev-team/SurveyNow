using Application.DTOs.Response.Point;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappers
{
    public class PointMappingProfile : Profile
    {
        public PointMappingProfile()
        {
            #region Purchase
            CreateMap<PointHistory, PointPurchaseResponse>();
            CreateMap<PointHistory, PointPurchaseDetailResponse>();
            #endregion

            #region Redeem
            #endregion

            #region PointPurchase/Transaction
            CreateMap<PointPurchase, ShortPointPurchaseResponse>();
            #endregion
        }
    }
}

using Application.DTOs.Response.Point;
using Application.Utils;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappers
{
    public class PointMappingProfile : Profile
    {
        public PointMappingProfile()
        {
            #region Purchase
            CreateMap<PointHistory, PointPurchaseResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.Date)));

            CreateMap<PointHistory, PointPurchaseDetailResponse>()
                .ForMember(dest => dest.Date, 
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.Date)));
            #endregion

            #region Redeem
            CreateMap<PointHistory, PointRedeemDetailResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.Date)));

            #endregion

            #region PointPurchase/Transaction
            CreateMap<PointPurchase, ShortPointPurchaseResponse>();
            #endregion
        }
    }
}

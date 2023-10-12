using Application.DTOs.Response;
using Application.DTOs.Response.Momo;
using Application.DTOs.Response.Pack;
using Application.DTOs.Response.Point.History;
using Application.DTOs.Response.Survey;
using Application.DTOs.Response.Transaction;
using Application.Utils;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Mappers
{
    public class PointMappingProfile : Profile
    {
        public PointMappingProfile()
        {
            #region History
            CreateMap<PointHistory, BasePointHistoryResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<PointHistory, ShortPointHistoryResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<PagingResponse<PointHistory>, PagingResponse<ShortPointHistoryResponse>>();

            CreateMap<PointHistory, Application.DTOs.Response.Point.History.PointPurchaseDetailResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<PointHistory, Application.DTOs.Response.Point.History.PointRedeemDetailResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<PointHistory, Application.DTOs.Response.Point.History.PointDoSurveyDetailResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<PointHistory, Application.DTOs.Response.Point.History.PointPackPurchaseDetailResponse>()
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PointHistoryType,
                src => src.MapFrom(src => EnumUtil.ConvertPointHistoryTypeToString(src.PointHistoryType)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)));

            CreateMap<Transaction, TransactionResponse>()
                .ForMember(dest => dest.TransactionType,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionTypeToString(src.TransactionType)))
                .ForMember(dest => dest.PaymentMethod,
                src => src.MapFrom(src => Enum.GetName(src.PaymentMethod)))
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)))
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)));

            CreateMap<PagingResponse<Transaction>, PagingResponse<TransactionResponse>>();

            CreateMap<Survey, ShortSurveyResponse>()
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertSurveyStatusToString(src.Status)));

            CreateMap<PackPurchase, PackPurchaseResponse>()
                .ForMember(dest => dest.Status,
                src => src.MapFrom(src => EnumUtil.ConvertTransactionStatusToString(src.Status)))
                .ForMember(dest => dest.Date,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV1(src.Date)))
                .ForMember(dest => dest.PackType,
                src => src.MapFrom(src => Enum.GetName(src.PackType)));

            #endregion
            #region Momo
            CreateMap<MomoCreatePaymentResponse, MomoPaymentMethodResponse>();
            #endregion
        }
    }
}

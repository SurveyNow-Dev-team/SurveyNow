using Application.DTOs.Request.Survey;
using Application.DTOs.Response.Survey;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Mappers;

public class SurveyMappingProfile : Profile
{
    public SurveyMappingProfile()
    {
        CreateMap<RowOptionRequest, RowOption>();
        CreateMap<ColumnOptionRequest, ColumnOption>();
        CreateMap<QuestionRequest, Question>();
        CreateMap<SectionRequest, Section>();
        CreateMap<SurveyRequest, Survey>();
        CreateMap<SectionRequest, Section>();
        CreateMap<Survey, SurveyDetailResponse>()
            .ForMember(dest => dest.StartDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.ExpiredDate)))
            .ForMember(dest => dest.CreatedDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.CreatedDate)))
            .ForMember(dest => dest.ModifiedDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.ModifiedDate)));
        CreateMap<Section, SectionResponse>();
        CreateMap<Question, QuestionResponse>();
        CreateMap<RowOption, RowOptionResponse>();
        CreateMap<ColumnOption, ColumnOptionResponse>();
    }
}
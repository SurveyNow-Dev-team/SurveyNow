using Application.DTOs.Request.Survey;
using Application.DTOs.Response;
using Application.DTOs.Response.Survey;
using Application.Utils;
using AutoMapper;
using Domain.Entities;

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
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.ModifiedDate)))
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(src => src.CreatedBy.FullName));
        CreateMap<Section, SectionResponse>();
        CreateMap<Question, QuestionResponse>();
        CreateMap<RowOption, RowOptionResponse>();
        CreateMap<ColumnOption, ColumnOptionResponse>();
        CreateMap<Survey, SurveyResponse>()
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(src => src.CreatedBy.FullName))
            .ForMember(dest => dest.CreatedDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.CreatedDate)))
            .ForMember(dest => dest.ModifiedDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.ModifiedDate)))
            .ForMember(dest => dest.StartDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDatetimeV2(src.ExpiredDate)));

        CreateMap<Survey, CommonSurveyResponse>()
            .ForMember(dest => dest.StartDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDateV1(src.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(src => DateUtil.FormatDateTimeToDateV1(src.ExpiredDate)))
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(src => src.CreatedBy.FullName));

        CreateMap<PagingResponse<Survey>, PagingResponse<SurveyResponse>>();
        CreateMap<PagingResponse<Survey>, PagingResponse<CommonSurveyResponse>>();

        CreateMap<AnswerRequest, Answer>();
        CreateMap<AnswerOptionRequest, AnswerOption>();

        CreateMap<AnswerOptionResponse, AnswerOption>();
        CreateMap<AnswerResponse, Answer>();
        
    }
}
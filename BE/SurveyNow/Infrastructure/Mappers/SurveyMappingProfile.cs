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
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.ExpiredDate)))
            .ForMember(dest => dest.CreatedDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.CreatedDate)))
            .ForMember(dest => dest.ModifiedDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.ModifiedDate)))
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(s => s.CreatedBy.FullName));
        CreateMap<Section, SectionResponse>();
        CreateMap<Question, QuestionResponse>();
        CreateMap<RowOption, RowOptionResponse>();
        CreateMap<ColumnOption, ColumnOptionResponse>();
        CreateMap<Survey, SurveyResponse>()
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(s => s.CreatedBy.FullName))
            .ForMember(dest => dest.CreatedDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.CreatedDate)))
            .ForMember(dest => dest.ModifiedDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.ModifiedDate)))
            .ForMember(dest => dest.StartDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDatetimeV2(s.ExpiredDate)));

        CreateMap<Survey, CommonSurveyResponse>()
            .ForMember(dest => dest.StartDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDateV1(s.StartDate)))
            .ForMember(dest => dest.ExpiredDate,
                src => src.MapFrom(s => DateUtil.FormatDateTimeToDateV1(s.ExpiredDate)))
            .ForMember(dest => dest.CreatedUserFullName, src => src.MapFrom(s => s.CreatedBy.FullName));

        CreateMap<PagingResponse<Survey>, PagingResponse<SurveyResponse>>();
        CreateMap<PagingResponse<Survey>, PagingResponse<CommonSurveyResponse>>();

        CreateMap<AnswerRequest, Answer>();
        CreateMap<AnswerOptionRequest, AnswerOption>();

        CreateMap<AnswerOption, AnswerOptionResponse>();
        CreateMap<Answer, AnswerResponse>();

        CreateMap<UserSurvey, UserSurveyResponse>()
            .ForMember(dest => dest.Date, src => src.MapFrom(us => DateUtil.FormatDateTimeToDatetimeV2(us.Date)))
            .ForMember(dest => dest.FullName, src => src.MapFrom(us => us.User.FullName));

        CreateMap<PagingResponse<UserSurvey>, PagingResponse<UserSurveyResponse>>();

        CreateMap<CriterionRequest, Criterion>()
            .ForAllMembers(config => config.Condition((src, dest, value) => src != null));
    }
}
using Application.DTOs.Request.Survey;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Mappers;

public class SurveyMappingProfile : Profile
{
    public SurveyMappingProfile()
    {
        CreateMap<SurveyQuestionDetailRequest, QuestionDetail>()
            .ForMember(des => des.QuestionOrder,
                otp => otp.MapFrom<QuestionDetailResolver>());

        CreateMap<SurveyQuestionRequest, Question>()
            .ForMember(des => des.Order, 
                otp => otp.MapFrom<QuestionResolver>());

        CreateMap<CreateSurveyRequest, Survey>()
            .ForMember(des => des.TotalQuestion, 
                src => src.MapFrom(src => src.SurveyQuestionRequests.Count))
            .ForMember(des => des.Status, 
                src => src.MapFrom(src => SurveyStatus.Draft));
    }
}

public class QuestionDetailResolver : IValueResolver<SurveyQuestionDetailRequest, QuestionDetail, int>
{
    public int Resolve(SurveyQuestionDetailRequest source, QuestionDetail destination, int destMember,
        ResolutionContext context)
    {
        var sourceList = context.Items["SourceList"] as List<SurveyQuestionDetailRequest>;
        return sourceList.IndexOf(source) + 1;
    }
}

public class QuestionResolver : IValueResolver<SurveyQuestionRequest, Question, int>
{
    public int Resolve(SurveyQuestionRequest source, Question destination, int destMember, ResolutionContext context)
    {
        var sourceList = context.Items["SourceList"] as List<SurveyQuestionRequest>;
        return sourceList.IndexOf(source) + 1;
    }
}
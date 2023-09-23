using Application.DTOs.Request.Survey;
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
    }
}


//
// public class QuestionDetailResolver : IValueResolver<SurveyQuestionDetailRequest, QuestionDetail, int>
// {
//     public int Resolve(SurveyQuestionDetailRequest source, QuestionDetail destination, int destMember,
//         ResolutionContext context)
//     {
//         var sourceList = context.Items["SourceList"] as List<SurveyQuestionDetailRequest>;
//         return sourceList.IndexOf(source) + 1;
//     }
// }

// public class QuestionResolver : IValueResolver<SurveyQuestionRequest, Question, int>
// {
//     public int Resolve(SurveyQuestionRequest source, Question destination, int destMember, ResolutionContext context)
//     {
//         var sourceList = context.Items["SourceList"] as List<SurveyQuestionRequest>;
//         return sourceList.IndexOf(source) + 1;
//     }
// }
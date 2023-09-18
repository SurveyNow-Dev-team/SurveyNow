using Application.DTOs.Request.Survey;

namespace Application.Interfaces.Services;

public interface ISurveyService
{

    Task<long> CreateSurveyAsync(SurveyRequest request);
}
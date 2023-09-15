using Application.DTOs.Request.Survey;

namespace Application.Interfaces.Services;

public interface ISurveyService
{
    public Task<long> CreateSurveyAsync(CreateSurveyRequest request);
}
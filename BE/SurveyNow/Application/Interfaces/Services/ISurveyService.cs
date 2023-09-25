using Application.DTOs.Request.Survey;
using Application.DTOs.Response.Survey;

namespace Application.Interfaces.Services;

public interface ISurveyService
{
    Task<long> CreateSurveyAsync(SurveyRequest request);
    Task<SurveyDetailResponse> GetByIdAsync(long id);
}
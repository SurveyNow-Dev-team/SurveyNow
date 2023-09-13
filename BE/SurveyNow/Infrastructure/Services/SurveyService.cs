using Application.DTOs.Request.Survey;
using Application.Interfaces.Services;

namespace Infrastructure.Services;

public class SurveyService: ISurveyService
{
    public Task<long> CreateSurveyAsync(CreateSurveyRequest request)
    {
        throw new NotImplementedException();
    }
}
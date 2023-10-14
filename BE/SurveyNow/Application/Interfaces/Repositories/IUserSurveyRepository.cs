using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserSurveyRepository : IBaseRepository<UserSurvey>
{
    Task<UserSurvey?> GetBySurveyIdAndUserIdAsync(long surveyId, long userId);
    Task<bool> ExistBySurveyIdAndUserId(long surveyId, long userId);
}
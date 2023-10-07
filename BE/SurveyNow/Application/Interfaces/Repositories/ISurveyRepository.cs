using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISurveyRepository : IBaseRepository<Survey>
{
    public Task<Survey?> GetByIdWithoutTrackingAsync(long id);
    public Task<Survey?> GetSurveyAnswerAsync(long surveyId, long userId);
}
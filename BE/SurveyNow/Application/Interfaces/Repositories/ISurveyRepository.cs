using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface ISurveyRepository : IBaseRepository<Survey>
{
    public Task<Survey?> GetByIdWithoutTrackingAsync(long id);
    Task<Survey?> GetCommonSurveyById(long id);
    public Task<Survey?> GetSurveyAnswerAsync(long surveyId, long userId);

    public Task UpdateTotalParticipant(int id, int value);
    public Task<List<Survey>> GetExpiredSurvey();
}
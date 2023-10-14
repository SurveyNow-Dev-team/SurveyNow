using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserSurveyRepository : BaseRepository<UserSurvey>, IUserSurveyRepository
{
    public UserSurveyRepository(AppDbContext context, ILogger<BaseRepository<UserSurvey>> logger) : base(context,
        logger)
    {
    }

    public async Task<UserSurvey?> GetBySurveyIdAndUserIdAsync(long surveyId, long userId)
    {
        return await _dbSet.FirstOrDefaultAsync(us => (us.SurveyId == surveyId && us.UserId == userId));
    }

    public async Task<bool> ExistBySurveyIdAndUserId(long surveyId, long userId)
    {
        return await GetBySurveyIdAndUserIdAsync(surveyId, userId) != null;
    }
}
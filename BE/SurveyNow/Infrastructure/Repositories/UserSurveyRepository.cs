using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserSurveyRepository: BaseRepository<UserSurvey>, IUserSurveyRepository
{
    public UserSurveyRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
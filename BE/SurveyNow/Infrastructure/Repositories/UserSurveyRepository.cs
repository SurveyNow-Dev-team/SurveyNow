using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserSurveyRepository: BaseRepository<UserSurvey>, IUserSurveyRepository
{
    public UserSurveyRepository(AppDbContext context) : base(context)
    {
    }
}
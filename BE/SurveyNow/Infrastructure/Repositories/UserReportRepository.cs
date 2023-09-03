using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserReportRepository: BaseRepository<UserReport>, IUserReportRepository
{
    public UserReportRepository(AppDbContext context) : base(context)
    {
    }
}
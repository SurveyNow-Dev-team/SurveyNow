using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserReportRepository: BaseRepository<UserReport>, IUserReportRepository
{
    public UserReportRepository(AppDbContext context, ILogger<BaseRepository<UserReport>> logger) : base(context, logger)
    {
    }
}
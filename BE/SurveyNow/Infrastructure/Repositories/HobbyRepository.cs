using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class HobbyRepository: BaseRepository<Hobby>, IHobbyRepository
{
    public HobbyRepository(AppDbContext context, ILogger<BaseRepository<Hobby>> logger) : base(context, logger)
    {
    }
}
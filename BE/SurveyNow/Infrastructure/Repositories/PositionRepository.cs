using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PositionRepository: BaseRepository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
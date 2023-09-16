using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class OccupationRepository: BaseRepository<Occupation>, IOccupationRepository
{
    public OccupationRepository(AppDbContext context, ILogger<BaseRepository<Occupation>> logger) : base(context, logger)
    {
    }
}
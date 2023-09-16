using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class DistrictRepository: BaseRepository<District>, IDistrictRepository
{
    public DistrictRepository(AppDbContext context, ILogger<BaseRepository<District>> logger) : base(context, logger)
    {
    }
}
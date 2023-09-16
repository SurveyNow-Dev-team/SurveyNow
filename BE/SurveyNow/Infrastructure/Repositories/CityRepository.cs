using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(AppDbContext context, ILogger<BaseRepository<City>> logger) : base(context, logger)
    {
    }
}
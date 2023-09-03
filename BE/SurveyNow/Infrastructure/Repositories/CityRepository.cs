using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(AppDbContext context) : base(context)
    {
    }
}
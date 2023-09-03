using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class DistrictRepository: BaseRepository<District>, IDistrictRepository
{
    protected DistrictRepository(AppDbContext context) : base(context)
    {
    }
}
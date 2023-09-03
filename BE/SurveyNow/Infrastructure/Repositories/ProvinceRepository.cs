using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProvinceRepository: BaseRepository<Province>, IProvinceRepository
{
    protected ProvinceRepository(AppDbContext context) : base(context)
    {
    }
}
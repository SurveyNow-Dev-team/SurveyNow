using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ProvinceRepository: BaseRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(AppDbContext context) : base(context)
    {
    }
}
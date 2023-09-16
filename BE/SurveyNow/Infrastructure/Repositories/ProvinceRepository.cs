using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class ProvinceRepository: BaseRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(AppDbContext context, ILogger<BaseRepository<Province>> logger) : base(context, logger)
    {
    }
}
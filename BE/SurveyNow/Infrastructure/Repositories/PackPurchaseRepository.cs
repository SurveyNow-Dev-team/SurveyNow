using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PackPurchaseRepository: BaseRepository<PackPurchase>, IPackPurchaseRepository
{
    public PackPurchaseRepository(AppDbContext context, ILogger<BaseRepository<PackPurchase>> logger) : base(context, logger)
    {
    }
}
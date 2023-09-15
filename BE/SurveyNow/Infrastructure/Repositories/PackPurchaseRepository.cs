using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PackPurchaseRepository: BaseRepository<PackPurchase>, IPackPurchaseRepository
{
    public PackPurchaseRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class PackPurchaseRepository : BaseRepository<PackPurchase>, IPackPurchaseRepository
{
    public PackPurchaseRepository(AppDbContext context, ILogger<BaseRepository<PackPurchase>> logger) : base(context, logger)
    {
    }

    public async Task<PackPurchase> AddPackPurchaseAsync(PackPurchase packPurchase)
    {
        if (packPurchase == null)
        {
            throw new ArgumentNullException("Pack Purchase information is invalid!");
        }
        try
        {
            EntityEntry<PackPurchase> result = await _dbSet.AddAsync(packPurchase);
            return result.Entity;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Adding Pack Purchase Task failed.", ex);
        }
    }
}
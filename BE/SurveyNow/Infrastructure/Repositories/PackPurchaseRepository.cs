using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PackPurchaseRepository: BaseRepository<PackPurchase>, IPackPurchaseRepository
{
    protected PackPurchaseRepository(AppDbContext context) : base(context)
    {
    }
}
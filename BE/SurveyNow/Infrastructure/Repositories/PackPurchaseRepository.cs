using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PackPurchaseRepository: BaseRepository<PackPurchase>, IPackPurchaseRepository
{
    public PackPurchaseRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PointPurchaseRepository: BaseRepository<PointPurchase>, IPointPurchaseRepository
{
    protected PointPurchaseRepository(AppDbContext context) : base(context)
    {
    }
}
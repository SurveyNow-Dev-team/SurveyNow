using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PointPurchaseRepository: BaseRepository<PointPurchase>, IPointPurchaseRepository
{
    public PointPurchaseRepository(AppDbContext context) : base(context)
    {
    }
}
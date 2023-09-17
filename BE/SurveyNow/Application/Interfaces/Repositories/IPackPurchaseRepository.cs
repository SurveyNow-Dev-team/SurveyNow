using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPackPurchaseRepository: IBaseRepository<PackPurchase>
{
    Task<PackPurchase> AddPackPurchaseAsync(PackPurchase packPurchase);
}
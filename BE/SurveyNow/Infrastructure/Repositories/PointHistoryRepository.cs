using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PointHistoryRepository : BaseRepository<PointHistory>, IPointHistoryRepository
{
    public PointHistoryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PointHistory> GetPointPurchaseDetailAsync(long id)
    {
        return await _dbSet.Include(p => p.PointPurchase)
                        .FirstOrDefaultAsync(p => p.Id == id);
    }
}
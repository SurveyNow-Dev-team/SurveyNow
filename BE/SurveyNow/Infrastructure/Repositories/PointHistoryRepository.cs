using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PointHistoryRepository: BaseRepository<PointHistory>, IPointHistoryRepository
{
    protected PointHistoryRepository(AppDbContext context) : base(context)
    {
    }
}
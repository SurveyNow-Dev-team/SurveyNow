using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PointHistoryRepository: BaseRepository<PointHistory>, IPointHistoryRepository
{
    public PointHistoryRepository(AppDbContext context) : base(context)
    {
    }
}
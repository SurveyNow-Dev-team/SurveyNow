using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class PositionRepository: BaseRepository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class RowOptionRepository:BaseRepository<RowOption>, IRowOptionRepository
{
    public RowOptionRepository(AppDbContext context) : base(context)
    {
    }
}
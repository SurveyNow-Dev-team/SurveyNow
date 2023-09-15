using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class ColumnOptionRepository:BaseRepository<ColumnOption>, IColumnOptionRepository
{
    public ColumnOptionRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class FieldRepository: BaseRepository<Field>, IFieldRepository
{
    public FieldRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class FieldRepository: BaseRepository<Field>, IFieldRepository
{
    protected FieldRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class FieldRepository: BaseRepository<Field>, IFieldRepository
{
    public FieldRepository(AppDbContext context, ILogger<BaseRepository<Field>> logger) : base(context, logger)
    {
    }
}
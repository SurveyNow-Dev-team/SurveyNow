using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class RowOptionRepository:BaseRepository<RowOption>, IRowOptionRepository
{
    public RowOptionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CriterionRepository:BaseRepository<Criterion>, ICriterionRepository
{
    public CriterionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
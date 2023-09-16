using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AreaCriterionRepository:BaseRepository<AreaCriterion>, IAreaCriterionRepository
{
    public AreaCriterionRepository(AppDbContext context, ILogger<BaseRepository<AreaCriterion>> logger) : base(context, logger)
    {
    }
}
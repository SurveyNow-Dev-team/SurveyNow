using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class GenderCriterionRepository:BaseRepository<GenderCriterion>, IGenderCriterionRepository
{
    public GenderCriterionRepository(AppDbContext context, ILogger<BaseRepository<GenderCriterion>> logger) : base(context, logger)
    {
    }
}
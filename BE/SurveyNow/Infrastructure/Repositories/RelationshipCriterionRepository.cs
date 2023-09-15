using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class RelationshipCriterionRepository:BaseRepository<RelationshipCriterion>, IRelationshipCriterionRepository
{
    public RelationshipCriterionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
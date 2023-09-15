using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class RelationshipCriterionRepository:BaseRepository<RelationshipCriterion>, IRelationshipCriterionRepository
{
    public RelationshipCriterionRepository(AppDbContext context) : base(context)
    {
    }
}
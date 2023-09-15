using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class FieldCriterionRepository:BaseRepository<FieldCriterion>, IFieldCriterionRepository
{
    public FieldCriterionRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class AreaCriterionRepository:BaseRepository<AreaCriterion>, IAreaCriterionRepository
{
    public AreaCriterionRepository(AppDbContext context) : base(context)
    {
    }
}
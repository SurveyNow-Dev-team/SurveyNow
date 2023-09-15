using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class GenderCriterionRepository:BaseRepository<GenderCriterion>, IGenderCriterionRepository
{
    public GenderCriterionRepository(AppDbContext context) : base(context)
    {
    }
}
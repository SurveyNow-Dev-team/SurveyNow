using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CriterionRepository:BaseRepository<Criterion>, ICriterionRepository
{
    public CriterionRepository(AppDbContext context) : base(context)
    {
    }
}
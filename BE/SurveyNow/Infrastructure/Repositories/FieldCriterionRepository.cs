using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class FieldCriterionRepository:BaseRepository<FieldCriterion>, IFieldCriterionRepository
{
    public FieldCriterionRepository(AppDbContext context, ILogger<BaseRepository<FieldCriterion>> logger) : base(context, logger)
    {
    }
}
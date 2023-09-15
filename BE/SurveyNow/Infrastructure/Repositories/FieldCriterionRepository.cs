using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class FieldCriterionRepository:BaseRepository<FieldCriterion>, IFieldCriterionRepository
{
    public FieldCriterionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
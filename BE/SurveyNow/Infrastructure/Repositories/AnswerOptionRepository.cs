using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AnswerOptionRepository : BaseRepository<AnswerOption>, IAnswerOptionRepository
{
    public AnswerOptionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
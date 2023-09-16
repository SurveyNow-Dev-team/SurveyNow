using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class AnswerRepository: BaseRepository<Answer>, IAnswerRepository
{
    public AnswerRepository(AppDbContext context, ILogger<BaseRepository<Answer>> logger) : base(context, logger)
    {
    }
}
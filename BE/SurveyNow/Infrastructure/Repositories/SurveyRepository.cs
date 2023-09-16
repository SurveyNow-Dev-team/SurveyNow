using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SurveyRepository: BaseRepository<Survey>, ISurveyRepository
{
    public SurveyRepository(AppDbContext context, ILogger<BaseRepository<Survey>> logger) : base(context, logger)
    {
    }
}
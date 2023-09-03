using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class SurveyRepository: BaseRepository<Survey>, ISurveyRepository
{
    public SurveyRepository(AppDbContext context) : base(context)
    {
    }
}
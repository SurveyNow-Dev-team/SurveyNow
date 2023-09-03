using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class AnswerRepository: BaseRepository<Answer>, IAnswerRepository
{
    protected AnswerRepository(AppDbContext context) : base(context)
    {
    }
}
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class QuestionRepository: BaseRepository<Question>, IQuestionRepository
{
    protected QuestionRepository(AppDbContext context) : base(context)
    {
    }
}
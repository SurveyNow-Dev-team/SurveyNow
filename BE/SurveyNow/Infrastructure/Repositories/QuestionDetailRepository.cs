using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class QuestionDetailRepository: BaseRepository<QuestionDetail>, IQuestionDetailRepository
{
    protected QuestionDetailRepository(AppDbContext context) : base(context)
    {
    }
}
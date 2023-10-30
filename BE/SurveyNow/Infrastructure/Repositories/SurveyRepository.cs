using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SurveyRepository : BaseRepository<Survey>, ISurveyRepository
{
    public SurveyRepository(AppDbContext context, ILogger<BaseRepository<Survey>> logger) : base(context, logger)
    {
    }

    public new async Task<Survey?> GetByIdAsync(object id)
    {
        if (id is long longId)
        {
            return await _dbSet.Include(s => s.Sections.OrderBy(s => s.Order))
                .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
                .ThenInclude(q => q.RowOptions.OrderBy(ro => ro.Order))
                .Include(s => s.Sections.OrderBy(s => s.Order))
                .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
                .ThenInclude(q => q.ColumnOptions.OrderBy(co => co.Order))
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(s => s.Id == longId);
        }

        return null;
    }

    public new async Task<List<Survey>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.CreatedBy)
            .ToListAsync();
    }

    public async Task<Survey?> GetByIdWithoutTrackingAsync(long id)
    {
        return await _dbSet.AsNoTracking().Include(s => s.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.RowOptions.OrderBy(ro => ro.Order))
            .Include(s => s.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.ColumnOptions.OrderBy(co => co.Order))
            .Include(s => s.CreatedBy)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Survey?> GetSurveyAnswerAsync(long surveyId, long userId)
    {
        return await _dbSet.AsNoTracking()
            .Include(s => s.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.RowOptions.OrderBy(ro => ro.Order))
            .Include(s => s.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.ColumnOptions.OrderBy(co => co.Order))
            .Include(s => s.Sections.OrderBy(s => s.Order))
            .ThenInclude(s => s.Questions.OrderBy(q => q.Order))
            .ThenInclude(q => q.Answers.Where(a => a.UserId == userId))
            .ThenInclude(a => a.AnswerOptions.OrderBy(ao => ao.RowOrder).ThenBy(ao => ao.ColumnOrder))
            .Include(s => s.CreatedBy)
            .FirstOrDefaultAsync(s => s.Id == surveyId);
    }

    public async Task UpdateTotalParticipant(int id, int value)
    {
        var survey = await GetByIdAsync(id);
        if (survey != null)
        {
            if (value > 0)
            {
                survey.TotalParticipant = value;
                Update(survey);
            }
        }
    }

    public async Task<List<Survey>> GetExpiredSurvey()
    {
        return await _dbSet.Where(s => s.Status == Domain.Enums.SurveyStatus.Active
        && DateTime.Compare((DateTime)s.ExpiredDate, DateTime.UtcNow) < 0)
            .ToListAsync();
    }
}
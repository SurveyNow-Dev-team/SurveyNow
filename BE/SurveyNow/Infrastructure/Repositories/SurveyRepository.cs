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

    public async Task<Survey?> GetByIdAsync(object id)
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

    public async Task<List<Survey>> GetAllAsync()
    {
        return await _dbSet
            .Include(s => s.CreatedBy)
            .ToListAsync();
    }

    public async Task<Survey> GetByIdWithoutTrackingAsync(long id)
    {
        if (id is long longId)
        {
            return await _dbSet.AsNoTracking().Include(s => s.Sections.OrderBy(s => s.Order))
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
}
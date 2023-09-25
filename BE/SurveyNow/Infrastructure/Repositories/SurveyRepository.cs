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

    public Task<Survey?> GetByIdAsync(object id)
    {
        if (id is long longId)
        {
            return _dbSet.Include(s => s.Sections).ThenInclude(s => s.Questions)
                .ThenInclude(q => q.RowOptions)
                .Include(s => s.Sections)
                .ThenInclude(s => s.Questions)
                .ThenInclude(q => q.ColumnOptions)
                .FirstOrDefaultAsync(s => s.Id == longId);
        }

        return null;
    }
}
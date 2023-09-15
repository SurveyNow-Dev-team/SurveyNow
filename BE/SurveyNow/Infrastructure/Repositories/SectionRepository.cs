using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }
}
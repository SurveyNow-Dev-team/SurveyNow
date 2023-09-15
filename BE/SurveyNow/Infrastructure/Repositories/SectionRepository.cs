using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class SectionRepository : BaseRepository<Section>, ISectionRepository
{
    public SectionRepository(AppDbContext context) : base(context)
    {
    }
}
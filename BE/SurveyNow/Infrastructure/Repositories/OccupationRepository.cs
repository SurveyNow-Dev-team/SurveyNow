using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class OccupationRepository: BaseRepository<Occupation>, IOccupationRepository
{
    protected OccupationRepository(AppDbContext context) : base(context)
    {
    }
}
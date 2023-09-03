using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class OccupationRepository: BaseRepository<Occupation>, IOccupationRepository
{
    public OccupationRepository(AppDbContext context) : base(context)
    {
    }
}
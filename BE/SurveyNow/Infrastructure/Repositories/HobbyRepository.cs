using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class HobbyRepository: BaseRepository<Hobby>, IHobbyRepository
{
    protected HobbyRepository(AppDbContext context) : base(context)
    {
    }
}
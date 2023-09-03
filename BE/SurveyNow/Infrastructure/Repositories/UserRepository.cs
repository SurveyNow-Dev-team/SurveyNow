using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class UserRepository: BaseRepository<User>, IUserRepository
{
    protected UserRepository(AppDbContext context) : base(context)
    {
    }
}
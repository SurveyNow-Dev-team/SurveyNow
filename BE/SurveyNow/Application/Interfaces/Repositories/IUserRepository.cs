using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository: IBaseRepository<User>
{
    public Task<User?> GetByEmailAsync(string email);

    public Task<User?> GetByEmailAndPasswordAsync(string email, string password);

    public Task<bool> ExistByEmail(string email);
}
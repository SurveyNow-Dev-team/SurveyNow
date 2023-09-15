using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context, ILogger logger) : base(context, logger)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var normalizeEmail = email.Trim().ToLower();
        return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(normalizeEmail));
    }

    public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
    {
        var user = await GetByEmailAsync(email);
        if (user != null)
        {
            if (BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
                return user;
        }
        return null;
    }

    public async Task<bool> ExistByEmail(string email)
    {
        var user = await GetByEmailAsync(email);
        return user != null;
    }
}
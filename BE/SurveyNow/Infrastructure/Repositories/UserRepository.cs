using Application.ErrorHandlers;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context, ILogger<BaseRepository<User>> logger) : base(context, logger)
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
            if (user.IsDelete)
            {
                throw new ForbiddenException("This account has been deleted.");
            }

            if (user.Status == UserStatus.InActive)
            {
                throw new ForbiddenException("This account has been blocked.");
            }

            if (user.Status == UserStatus.Suspending)
            {
                throw new ForbiddenException("This account is suspending.");
            }

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

    public async Task UpdateUserPoint(long userId, UserPointAction pointAction, decimal pointAmount)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("Invalid UserId");
        }

        if (pointAmount <= 0)
        {
            throw new ArgumentException("Point must be a positive value");
        }

        var user = await GetByIdAsync(userId);
        if (user == null)
        {
            throw new NullReferenceException($"Cannot find user with the given ID: {userId}");
        }

        switch (pointAction)
        {
            case UserPointAction.IncreasePoint:
                user.Point += pointAmount;
                Update(user);
                break;
            case UserPointAction.DecreasePoint:
                user.Point -= pointAmount;
                if (user.Point < 0)
                {
                    user.Point = 0;
                }

                Update(user);
                break;
            // Handle additional actions here
            default:
                throw new ArgumentException("Invalid point action");
        }
    }
}
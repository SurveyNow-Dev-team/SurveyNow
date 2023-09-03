using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync(User user);
}
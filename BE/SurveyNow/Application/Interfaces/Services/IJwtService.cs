using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces.Services;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync(User user);
    ClaimsPrincipal ConvertToken(string token);
}
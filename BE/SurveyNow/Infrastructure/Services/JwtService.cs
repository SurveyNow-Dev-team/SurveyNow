using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Configurations;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly AppConfiguration configuration;

    public JwtService(AppConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        //create claims:
        var claims = await Task.Run(() => new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("role", user.Role.ToString())
            }
        );

        //create signing key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //create token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(User user)
    {
        //create claims:
        var claims = await Task.Run(() => new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("role", user.Role.ToString())
            }
        );

        //create signing key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //create token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
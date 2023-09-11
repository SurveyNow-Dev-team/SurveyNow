using Domain.Enums;

namespace Application.DTOs.Response;

public record LoginUserResponse
{
    public long? Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public Gender? Gender { get; set; }
    public string? AvatarUrl { get; set; }
    public decimal Point { get; set; }

    public UserStatus Status { get; set; } = UserStatus.Active;

    public Role Role { get; set; } = Role.User;

    public string? LangKey { get; set; }

    public string? Currency { get; set; }

    public bool IsDelete { get; set; } = false;

    public string Token { get; set; } = null!;

}
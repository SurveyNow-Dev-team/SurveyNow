using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.User;

public record LoginUserRequest
{
    [Required(ErrorMessage = "Email is required!")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Password is required!")]
    public string Password { get; init; } = null!;
}
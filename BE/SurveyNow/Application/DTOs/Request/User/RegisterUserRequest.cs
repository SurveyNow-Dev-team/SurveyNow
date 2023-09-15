using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.User;

public record RegisterUserRequest()
{
   [Required (ErrorMessage = "Email is required!")]
   [DataType(DataType.EmailAddress)]
   public string Email { get; init; } = null!;

   [Required(ErrorMessage = "Password is required!")]
   [MinLength(8, ErrorMessage = "Password must has at least 8 character!")]
   public string Password { get; init; } = null!;

}
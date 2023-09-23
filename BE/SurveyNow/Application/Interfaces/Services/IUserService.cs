using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUser(long id);
        Task<PagingResponse<UserResponse>> GetUsers(UserRequest filter, PagingRequest pagingRequest);
        Task<UserResponse> UpdateUser(long id, UserRequest request);
        Task<LoginUserResponse> CreateUserAsync(RegisterUserRequest request);
        Task<LoginUserResponse> LoginAsync(LoginUserRequest request);
        Task<User?> GetCurrentUserAsync();
        Task UpdatePhoneNumber(string phoneNumber);
        Task VerifyPhoneNumber(string confirmedOtp);
        Task ChangePasswordAsync(PasswordChangeRequest request);
        Task Remove();
        Task UpdateAvatar(Stream stream, string fileName);
    }
}

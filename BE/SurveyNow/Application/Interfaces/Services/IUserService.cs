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
        Task<PagingResponse<UserResponse>> GetUsers(UserFilterRequest filter, PagingRequest pagingRequest);
        Task<UserResponse> UpdateUser(long id, UserRequest request);
        Task<UserResponse> UpdateCurrentUser(UserRequest request);
        Task<LoginUserResponse> CreateUserAsync(RegisterUserRequest request);
        Task<LoginUserResponse> LoginAsync(LoginUserRequest request);
        Task<LoginUserResponse> LoginWithGoogle(string idToken);
        Task<User?> GetCurrentUserAsync();
        Task UpdatePhoneNumber(string phoneNumber);
        Task VerifyPhoneNumber(string confirmedOtp);
        Task ChangePasswordAsync(PasswordChangeRequest request);
        Task Remove();
        Task<string> UpdateAvatar(Stream stream, string fileName);
        Task ChangeRole(long id, string role);
        Task ChangeStatus(long id, string status);
        Task<UserResponse> GetLoggedInUser();
    }
}

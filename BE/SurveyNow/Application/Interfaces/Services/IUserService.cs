using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Domain.Entities;

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
        Task ChangePasswordAsync(PasswordChangeRequest request);
        Task Remove();
    }
}

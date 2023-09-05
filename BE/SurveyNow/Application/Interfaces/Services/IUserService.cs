using Application.DTOs.Request;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUser(long id);
        Task<PagingResponse<UserResponse>> GetUsers(UserRequest filter, PagingRequest pagingRequest);
    }
}

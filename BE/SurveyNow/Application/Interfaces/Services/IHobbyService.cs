using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IHobbyService
    {
        Task<HobbyResponse> GetHobby(long id);
        Task CreateHobby(HobbyRequest request);
        Task<HobbyResponse> UpdateHobby(long id, HobbyRequest request);
    }
}

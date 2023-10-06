using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<AddressResponse> GetAddress(long id);
        Task CreateAddress(AddressRequest request);
        Task<AddressResponse> UpdateAddress(long id, AddressRequest request);
    }
}

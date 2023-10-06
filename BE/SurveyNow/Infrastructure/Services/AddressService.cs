using Application;
using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAddress(AddressRequest request)
        {
            await _unitOfWork.AddressRepository.AddAsync(_mapper.Map<Address>(request));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<AddressResponse> GetAddress(long id)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(id);
            return _mapper.Map<AddressResponse>(address);
        }

        public async Task<AddressResponse> UpdateAddress(long id, AddressRequest request)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(id);
            if(address != null)
            {
                address = _mapper.Map(request, address);
                _unitOfWork.AddressRepository.Update(address);
                await _unitOfWork.SaveChangeAsync();
            }
            return _mapper.Map<AddressResponse>(address);
        }
    }
}

using Application;
using Application.DTOs.Request;
using Application.DTOs.Response;
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
    public class HobbyService : IHobbyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HobbyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HobbyResponse> GetHobby(long id)
        {
            var hobby = await _unitOfWork.HobbyRepository.GetByIdAsync(id);
            return _mapper.Map<HobbyResponse>(hobby);
        }

        public async Task CreateHobby(HobbyRequest request)
        {
            await _unitOfWork.HobbyRepository.AddAsync(_mapper.Map<Hobby>(request));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<HobbyResponse> UpdateHobby(long id, HobbyRequest request)
        {
            var hobby = await _unitOfWork.HobbyRepository.GetByIdAsync(id);
            if (hobby != null)
            {
                hobby = _mapper.Map(request, hobby);
                _unitOfWork.HobbyRepository.Update(hobby);
                await _unitOfWork.SaveChangeAsync();
            }
            return _mapper.Map<HobbyResponse>(hobby);
        }
    }
}

using Application;
using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using Application.ErrorHandlers;
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
    public class OccupationService : IOccupationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OccupationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FieldDTO>> GetFields()
        {
            var fields = await _unitOfWork.FieldRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FieldDTO>>(fields);
        }

        public async Task CreateOccupation(OccupationRequest request)
        {
            await _unitOfWork.OccupationRepository.AddAsync(_mapper.Map<Occupation>(request));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<OccupationResponse> GetOccupation(long id)
        {
            var occupation = await _unitOfWork.OccupationRepository.GetByIdAsync(id, "Field");
            return _mapper.Map<OccupationResponse>(occupation);
        }

        public async Task<IEnumerable<OccupationResponse>> GetOccupations()
        {
            var occupations = await _unitOfWork.OccupationRepository.Get(includeProperties: "Field", filter: null, orderBy: null);
            return _mapper.Map<IEnumerable<OccupationResponse>>(occupations);
        }

        public async Task<OccupationResponse> UpdateOccupation(long id, OccupationRequest request)
        {
            var occupation = await _unitOfWork.OccupationRepository.GetByIdAsync(id);
            if(occupation == null)
            {
                throw new NotFoundException("Occupation not found");
            }
            occupation = _mapper.Map(request, occupation);
            _unitOfWork.OccupationRepository.Update(occupation);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<OccupationResponse>(occupation);
        }
    }
}

using Application;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagingResponse<UserResponse>> GetUsers(UserRequest filter, PagingRequest pagingRequest)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsync();
            return _mapper.Map<PagingResponse<User>, PagingResponse<UserResponse>>(user.AsQueryable<User>()
                .Filter<User>(_mapper.Map<User>(filter)).ToList<User>()
                .Paginate<User>(pagingRequest.Page, pagingRequest.RecordsPerPage));
        }

        public async Task<UserResponse> GetUser(long id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponse>(user);
        }
    }
}

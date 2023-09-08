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
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var userResponses = _mapper.Map<List<User>, List<UserResponse>>(users);
            var filteredUsers = userResponses.AsQueryable().Filter(_mapper.Map<UserResponse>(filter));
            var paginatedUsers = filteredUsers.ToList().Paginate(pagingRequest.Page, pagingRequest.RecordsPerPage);
            return paginatedUsers;
        }

        public async Task<UserResponse> GetUser(long id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(long id, UserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            user = _mapper.Map<UserRequest, User>(request, user);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<UserResponse>(user);
        }
    }
}

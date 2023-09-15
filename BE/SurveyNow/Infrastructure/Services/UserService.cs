using System.Security.Claims;
using Application;
using Application.DTOs.Request;
using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using Application.ErrorHandlers;
using Application.Interfaces.Services;
using Application.Utils;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<LoginUserResponse> CreateUserAsync(RegisterUserRequest request)
        {
            if (await _unitOfWork.UserRepository.ExistByEmail(request.Email))
            {
                throw new BadRequestException("This email has been existed.");
            }

            var userObj = _mapper.Map<User>(request);

            await _unitOfWork.UserRepository.AddAsync(userObj);
            var success = await _unitOfWork.SaveChangeAsync();

            if (success <= 0)
            {
                _logger.LogError($"Error when adding the user to the database.\n Date: {DateTime.UtcNow}");
                throw new ConflictException("Error when adding the user to the database.");
            }

            var createdUserObj = await _unitOfWork.UserRepository.GetByIdAsync(userObj.Id);

            var mappingTask = Task.Run(() => _mapper.Map<LoginUserResponse>(createdUserObj));
            var tokenTask = _jwtService.GenerateAccessTokenAsync(createdUserObj);
            await Task.WhenAll(mappingTask, tokenTask);

            var result = mappingTask.Result;
            var token = tokenTask.Result;

            result.Token = token;
            return result;
        }
        
        public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAndPasswordAsync(request.Email, request.Password);
            if (user == null)
            {
                throw new NotFoundException("Incorrect Email or Password");
            }
            var mappingTask = Task.Run(() => _mapper.Map<LoginUserResponse>(user));
            var tokenTask = _jwtService.GenerateAccessTokenAsync(user);
            await Task.WhenAll(mappingTask, tokenTask);

            var result = mappingTask.Result;
            var token = tokenTask.Result;

            result.Token = token;
            return result;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var subject = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(subject))
                return null;
            var fail = long.TryParse(subject, out var userId);
            return await _unitOfWork.UserRepository.GetByIdAsync(userId);
        }

        
    }
}
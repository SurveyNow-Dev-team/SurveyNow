using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly IPhoneNumberService _phoneNumberService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor, IPhoneNumberService phoneNumberService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _phoneNumberService = phoneNumberService;
        }

        public async Task<PagingResponse<UserResponse>> GetUsers(UserRequest filter, PagingRequest pagingRequest)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            if(users == null)
            {
                throw new NotFoundException("There aren't any users");
            }
            var userResponses = _mapper.Map<List<User>, List<UserResponse>>(users);
            var filteredUsers = userResponses.AsQueryable().Filter(_mapper.Map<UserResponse>(filter));
            var paginatedUsers = filteredUsers.ToList().Paginate(pagingRequest.Page, pagingRequest.RecordsPerPage);
            return paginatedUsers;
        }

        public async Task<UserResponse> GetUser(long id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User is not existed");
            }
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateUser(long id, UserRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User with this id is not existed");
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

        private async Task<User> GetLoggedInUserAsync()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers.First(x => x.Key.Equals("Authorization")).Value;
                var principle = _jwtService.ConvertToken(Regex.Replace(token, "Bearer ", ""));
                var userId = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _unitOfWork.UserRepository.GetByIdAsync(long.Parse(userId));
                if (user == null)
                {
                    throw new NotFoundException("User doesn't exist");
                }
                return user;
            }
            else
            {
                throw new NotFoundException("User doesn't log in");
            }
        }

        public async Task Remove()
        {
            var user = await GetLoggedInUserAsync();
            user.IsDelete = true;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdatePhoneNumber(string phoneNumber)
        {
            var user = await GetLoggedInUserAsync();
            if (user.PhoneNumber == null || !user.PhoneNumber.Equals(phoneNumber))
            {
                user.PhoneNumber = phoneNumber;
                var otp = GenerateOTP();
                await _phoneNumberService.SendSmsAsync($"Your SurveyNow verification code is: {otp}", phoneNumber);
                var session = _httpContextAccessor.HttpContext.Session;
                session.Set("OTP", Encoding.UTF8.GetBytes(otp));
                session.Set("PhoneNumber", Encoding.UTF8.GetBytes(phoneNumber));
            }
            else
            {
                throw new BadRequestException("Your phone number is still the same");
            }
        }

        public async Task VerifyPhoneNumber(string confirmedOtp)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.TryGetValue("OTP", out byte[] otp);
            session.TryGetValue("PhoneNumber", out byte[] phoneNumber);
            if(confirmedOtp.Equals(Encoding.UTF8.GetString(otp)))
            {
                var user = await GetLoggedInUserAsync();
                user.PhoneNumber = Encoding.UTF8.GetString(phoneNumber);
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                throw new BadRequestException("Wrong OTP");
            }
            session.Remove("OTP");
            session.Remove("PhoneNumber");
        }

        public async Task ChangePasswordAsync(PasswordChangeRequest request)
        {
            var user = await GetLoggedInUserAsync();
            if(!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.PasswordHash))
            {
                throw new BadRequestException("Old password is incorrect");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
        }

        private string GenerateOTP()
        {
            string otp = "";
            Random random = new Random();
            for(int i = 0; i < 6; i++)
            {
                otp += random.Next(0, 10);
            }
            return otp;
        }

    }
}
using System.Data;
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
using Domain.Enums;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio.Jwt.AccessToken;

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
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger, IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor, IPhoneNumberService phoneNumberService, IFileService fileService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
            _phoneNumberService = phoneNumberService;
            _fileService = fileService;
            _configuration = configuration;
        }

        public async Task<PagingResponse<UserResponse>> GetUsers(UserFilterRequest filter, PagingRequest pagingRequest)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(entityFilter: _mapper.Map<User>(filter), filter: x => x.Role != Role.Admin);
            if(users == null)
            {
                throw new NotFoundException("There aren't any users satisfied the criteria");
            }
            var userResponses = _mapper.Map<IEnumerable<User>, IEnumerable<UserResponse>>(users);
            var paginatedUsers = userResponses.ToList().Paginate(pagingRequest.Page, pagingRequest.RecordsPerPage);
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
            if (user.Role == Role.Admin)
            {
                throw new BadRequestException("Can't update admin account");
            }
            user = _mapper.Map<UserRequest, User>(request, user);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> UpdateCurrentUser(UserRequest request)
        {
            User user = await GetLoggedInUserAsync();
            if (user.Role == Role.Admin)
            {
                throw new BadRequestException("Can't update admin account");
            }
            user = _mapper.Map(request, user);
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
            if (user.IsDelete || user.Status != UserStatus.Active)
            {
                throw new UnauthorizedException("Your account is deleted or baned");
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
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("User hasn't logged in yet");
            }
            var user = await _unitOfWork.UserRepository.GetByIdAsync(long.Parse(userId), "Address,Occupation,Occupation.Field");
            if (user == null)
            {
                throw new NotFoundException("User doesn't exist");
            }
            return user;
        }

        public async Task Remove()
        {
            var user = await GetLoggedInUserAsync();
            if(user.Role == Role.Admin)
            {
                throw new BadRequestException("Can't remove admin account");
            }
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

        public async Task<string> UpdateAvatar(Stream stream, string fileName)
        {
            var user = await GetLoggedInUserAsync();
            string url = await _fileService.UploadFileAsync(stream, fileName);
            user.AvatarUrl = url;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
            return url;
        }

        public async Task ChangeRole(long id, string role)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.Role == Role.Admin)
            {
                throw new BadRequestException("Can't update admin account");
            }
            user.Role = _mapper.Map<Role>(role);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task ChangeStatus(long id, string status)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.Role == Role.Admin)
            {
                throw new BadRequestException("Can't update admin account");
            }
            user.Status = _mapper.Map<UserStatus>(status);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<UserResponse> GetLoggedInUser()
        {
            User user = await GetLoggedInUserAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<LoginUserResponse> LoginWithGoogle(string idToken)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();
            settings.Audience = new List<string> { _configuration.GetSection("Authentication:Google:ClientId").Value, "407408718192.apps.googleusercontent.com" };
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(payload.Email); 
            if (user == null)
            {
                user = new User() { 
                    Email = payload.Email,
                    FullName = payload.Name,
                    Role = Role.User,
                    AvatarUrl = payload.Picture
                };
                await _unitOfWork.UserRepository.AddAsync(user);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                if (user.IsDelete || user.Status != UserStatus.Active)
                {
                    throw new UnauthorizedException("Your account is deleted or baned");
                }
            }
            var response = _mapper.Map<LoginUserResponse>(user);
            response.Token = await _jwtService.GenerateAccessTokenAsync(user);
            return response;
        }
    }
}
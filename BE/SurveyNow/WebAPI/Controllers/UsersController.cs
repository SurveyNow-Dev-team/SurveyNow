using Application.DTOs.Request;
using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SurveyNow.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users, for Admin roles only
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pagingRequest"></param>
        /// <returns></returns>
        // GET: api/<UsersController>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResponse<UserResponse>>> Get([FromQuery] UserFilterRequest filter, [FromQuery] PagingRequest pagingRequest)
        {
            var users = await _userService.GetUsers(filter, pagingRequest);
            return Ok(users);
        }

        /// <summary>
        /// Get user by id, for Admin role only.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<UsersController>/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> Get(long id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        /// <summary>
        /// Get current login user
        /// </summary>
        /// <returns></returns>
        [HttpGet("login-user")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetLoggedInUser()
        {
            var user = await _userService.GetLoggedInUser();
            return Ok(user);
        }

        // PUT api/<UsersController>/5
        /// <summary>
        /// Update user basic information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> UpdateUser(long id, [FromBody] UserRequest userRequest)
        {
            var user = await _userService.UpdateUser(id, userRequest);
            return Ok(user);
        }

        /// <summary>
        /// Update current user password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("password")]
        [Authorize]
        public async Task ChangePassword([FromBody] PasswordChangeRequest request)
        {
            await _userService.ChangePasswordAsync(request);
        }

        /// <summary>
        /// Enter updated phone number & receive 6 number OTP to verify in that phone number's sms
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("phone-number")]
        public async Task UpdatePhoneNumber([FromBody][RegularExpression(@"^(84|0[3|5|7|8|9])[0-9]{8}$", ErrorMessage = "We currently support Vietnam phone number")] string phoneNumber)
        {
            await _userService.UpdatePhoneNumber(phoneNumber);
        }

        /// <summary>
        /// Enter OTP to verify phone number, if OTP is right, update phone number successfully
        /// </summary>
        /// <param name="confirmedOtp"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("phone-number-verification")]
        public async Task VerifyPhoneNumber([RegularExpression(@"^\d{6}$")] string confirmedOtp)
        {
            await _userService.VerifyPhoneNumber(confirmedOtp);
        }

        /// <summary>
        /// current user delete their account
        /// </summary>
        /// <returns></returns>
        // PUT api/<UsersController>/5
        [Authorize]
        [HttpPut("user-removal")]
        public async Task Remove()
        {
            await _userService.Remove();
        }

        /// <summary>
        /// Upload current user avatar
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("avatar")]
        public async Task<string> UploadAvatar(IFormFile formFile)
        {
            var url = await _userService.UpdateAvatar(formFile.OpenReadStream(), formFile.FileName);
            return url;
        }

        /// <summary>
        /// Change user role, allow Admin role only
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role">user has 2 role: User and Admin</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("role/{id}")]
        public async Task ChangeRole(long id, string role)
        {
            await _userService.ChangeRole(id, role);
        }

        /// <summary>
        /// Change user status, allow Admin role only
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status">user status includes 3 states: Active, InActive, and Suspending</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("status/{id}")]
        public async Task ChangeStatus(long id, string status)
        {
            await _userService.ChangeStatus(id, status);
        }
    }
}

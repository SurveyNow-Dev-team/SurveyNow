using Application.DTOs.Request;
using Application.DTOs.Request.User;
using Application.DTOs.Response;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SurveyNow.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<PagingResponse<UserResponse>>> Get([FromQuery]UserRequest filter, [FromQuery]PagingRequest pagingRequest)
        {
            var users = await _userService.GetUsers(filter, pagingRequest);
            if(users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> Get(long id)
        {
            var user = await _userService.GetUser(id);
            return Ok(user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> UpdateUser(long id, [FromBody] UserRequest userRequest)
        {
            var user = await _userService.UpdateUser(id, userRequest);
            return Ok(user);
        }

        [HttpPut("password-change")]
        public async Task ChangePassword([FromBody] PasswordChangeRequest request)
        {
            await _userService.ChangePasswordAsync(request);
        }

        // PUT api/<UsersController>/5
        [HttpPut("user-removal")]
        public async Task Remove()
        {
            await _userService.Remove();
        }
    }
}

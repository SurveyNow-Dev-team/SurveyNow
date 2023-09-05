using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SuerveyNow.Controllers
{
    [Route("api/user")]
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
        public async Task<ActionResult<PagingResponse<UserResponse>>> Get(UserRequest filter, PagingRequest pagingRequest)
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
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //// POST api/<UsersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void UpdateUser(int id, [FromBody] string value)
        {
        }

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

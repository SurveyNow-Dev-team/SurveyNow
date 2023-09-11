using Application.DTOs.Request.User;
using Application.DTOs.Response.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SurveyNow.Controllers;

[Route("api/v1/authentication")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginUserResponse>> Register([FromBody] RegisterUserRequest registerUser)
    {
        var createdUser = await _userService.CreateUserAsync(registerUser);
        return Ok(createdUser);
    }

    //Test get current User
    // [HttpGet("account")]
    // public async Task<ActionResult<User?>> GetCurrentUser()
    // {
    //     return Ok(await _userService.GetCurrentUserAsync());
    // }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserRequest requestUser)
    {
        var returnUser = await _userService.LoginAsync(requestUser);
        return Ok(returnUser);
    }
}
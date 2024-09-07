using Microsoft.AspNetCore.Mvc;
using mohaymen_codestar_Team02.Dto.UserDtos;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir.Controllers;

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("users")]  // Todo accessed by system admin
    public async Task<IActionResult> Register([FromBody] CreateUserDto request)
    {
        var user = new User
        {
            Username = request.Username,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        var response =
            await _authenticationService.Register(user, request.Password, request.Roles);

        return StatusCode((int)response.Type, response);
    }

    [HttpPost("login")] // Todo accessed by all users
    public async Task<IActionResult> Login([FromBody] LoginUserDto request)
    {
        var response = await _authenticationService.Login(request.Username, request.Password);
        return StatusCode((int)response.Type, response);
    }

    [HttpPost("logout")]  // Todo accessed by all users
    //[ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        var response = _authenticationService.Logout();
        return StatusCode((int)response.Type, response);
    }

    [HttpGet("permission")] // Todo accessed by all users
    public async Task<IActionResult> GetPermission()
    {
        var response = await _authenticationService.GetPermission();
        return StatusCode((int)response.Type, response);
    }

    [HttpGet] // Todo accessed by all users
    public async Task<IActionResult> GetAuthorized()
    {
        var response = await _authenticationService.GetAuthorized();
        return StatusCode((int)response.Type, response);
    }
}
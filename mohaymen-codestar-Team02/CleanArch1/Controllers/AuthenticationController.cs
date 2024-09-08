using Microsoft.AspNetCore.Mvc;
using WebApplication15.Dtos;
using WebApplication15.Services;

namespace WebApplication15.Controllers;

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    // sys admin
    [HttpPost("Authentication/Register")] // Todo accessed by system admin // use [controller]/[Action] format
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    { // better to pass user or fields separately
        var response = await _authenticationService.Register(request);
        return StatusCode((int)response.Type, response);
    } 
    
    // sys admin, data admin, analysis
    [HttpPost("Authentication/Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var response = await _authenticationService.Login(request);
        return StatusCode((int)response.Type, response);
    }

    // sys admin, data admin, analysis
    [HttpPost("Authentication/Logout")]
    public IActionResult Logout()
    {
        var response = _authenticationService.Logout();
        return StatusCode((int)response.Type, response);
    }
    
    // dk
    [HttpGet("Authentication/GetPermissions")]  // permissions or permission
    public async Task<IActionResult> GetPermission()
    {
        var response = await _authenticationService.GetPermission();
        return StatusCode((int)response.Type, response);
    }

    [HttpGet]  // why doesn't have url  // dk
    public async Task<IActionResult> GetAuthorized()
    {
        var response = await _authenticationService.GetAuthorized();
        return StatusCode((int)response.Type, response);
    }

}
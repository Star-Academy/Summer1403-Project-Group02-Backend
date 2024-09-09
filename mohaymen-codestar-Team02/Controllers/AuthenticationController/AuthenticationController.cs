using Microsoft.AspNetCore.Mvc;
using mohaymen_codestar_Team02.CleanArch1.Services.AuthenticationService.Abstraction;
using mohaymen_codestar_Team02.Dtos.AuthenticationDtos;

namespace mohaymen_codestar_Team02.Controllers.AuthenticationController;

public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticateionService _authenticationService;

    public AuthenticationController(IAuthenticateionService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    // sys admin
    [HttpPost("auth/register")] // Todo accessed by system admin // use [controller]/[Action] format
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    { // better to pass user or fields separately
        var response = await _authenticationService.Register(request);
        return StatusCode((int)response.Type, response);
    } 
    
    // sys admin, data admin, analysis
    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var response = await _authenticationService.Login(request);
        return StatusCode((int)response.Type, response);
    }

    // sys admin, data admin, analysis
    [HttpPost("auth/logout")]
    public IActionResult Logout()
    {
        var response = _authenticationService.Logout();
        return StatusCode((int)response.Type, response);
    }
    
    // dk
    [HttpGet("auth/permissions")]  // permissions or permission
    public async Task<IActionResult> GetPermission()
    {
        var response = await _authenticationService.GetPermission();
        return StatusCode((int)response.Type, response);
    }

    [HttpGet("auth/status")]  // why doesn't have url  // dk
    public async Task<IActionResult> GetAuthorized()
    {
        var response = await _authenticationService.GetAuthorized();
        return StatusCode((int)response.Type, response);
    }

}
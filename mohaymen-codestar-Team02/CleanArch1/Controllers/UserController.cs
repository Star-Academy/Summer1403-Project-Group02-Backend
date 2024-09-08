using Microsoft.AspNetCore.Mvc;
using WebApplication15.Dtos.UserDtos;
using WebApplication15.Services;

namespace WebApplication15.Controllers;

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("User/GetAllUsers")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber)
    {
        var response = await _userService.GetUsersPaginated(pageNumber);
        return StatusCode((int)response.Type, response);
    }

    [HttpGet("User/GetSingleUser/{userId}")]
    public async Task<IActionResult> GetSingleUser(long userId)
    {
        var response = await _userService.GetSingleUser(userId);
        return StatusCode((int)response.Type, response);
    }

    [HttpDelete("User/DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUser(long userId)
    {
        var response = await _userService.DeleteUser(userId);
        return StatusCode((int)response.Type, response);
    }

    [HttpPut("User/UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        var response = await _userService.UpdateUser(request);
        return StatusCode((int)response.Type, response);
    }
    
    [HttpPatch("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var response = await _userService.ChangePassword(request);
        return StatusCode((int)response.Type, response);
    }
}
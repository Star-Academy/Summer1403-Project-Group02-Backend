using Microsoft.AspNetCore.Mvc;
using mohaymen_codestar_Team02.CleanArch1.Services.RoleService.Abstraction;

namespace mohaymen_codestar_Team02.CleanArch1.Controllers.RoleController;

public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var response = await _roleService.GetAllRoles();
        return StatusCode((int)response.Type, response);
    }

    [HttpPut("roles/{userId}/{roleId}")] // post or put
    public async Task<IActionResult> AddUserRole(long userId, long roleId) // better to get from url or dto?
    {
        var response = await _roleService.AddUserRole(userId, roleId);
        return StatusCode((int)response.Type, response);
    }
    
    [HttpDelete("roles/{userId}/{roleId}")]
    public async Task<IActionResult> DeleteUserRole(long userId, long roleId)
    {
        var response = await _roleService.DeleteUserRole(userId, roleId);
        return StatusCode((int)response.Type, response);
    }
    
}
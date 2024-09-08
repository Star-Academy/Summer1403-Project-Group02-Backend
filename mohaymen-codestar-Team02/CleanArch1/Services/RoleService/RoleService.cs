using mohaymen_codestar_Team02.CleanArch1.Services.RoleService.Abstraction;
using mohaymen_codestar_Team02.Dto.Role;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.CleanArch1.Services.RoleService;

public class RoleService : IRoleService
{
    public Task<ServiceResponse<IEnumerable<GetRoleDto>>> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetUserDto?>> AddUserRole(long userId, long roleId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetUserDto?>> DeleteUserRole(long userId, long roleId)
    {
        throw new NotImplementedException();
    }
}
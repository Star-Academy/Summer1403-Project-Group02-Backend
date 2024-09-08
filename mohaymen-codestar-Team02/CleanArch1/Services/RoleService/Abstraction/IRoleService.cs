using mohaymen_codestar_Team02.Dto.Role;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.CleanArch1.Services.RoleService.Abstraction;

public interface IRoleService
{
    Task<ServiceResponse<IEnumerable<GetRoleDto>>> GetAllRoles();
    Task<ServiceResponse<GetUserDto?>> AddUserRole(long userId, long roleId);
    Task<ServiceResponse<GetUserDto?>> DeleteUserRole(long userId, long roleId);
}
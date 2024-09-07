using mohaymen_codestar_Team02.Dto.Role;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public interface ISystemManagmentService
{
    Task<ServiceResponse<IEnumerable<GetUserDto>?>> GetUsersPaginated(int pageNumber);
    Task<ServiceResponse<GetUserDto?>> GetUser(string? username);
    Task<ServiceResponse<GetUserDto?>> DeleteUser(User user);
    Task<ServiceResponse<GetUserDto?>> UpdateUser(User user);
    Task<ServiceResponse<IEnumerable<GetRoleDto>>> GetAllRoles();
    Task<ServiceResponse<GetUserDto?>> AddUserRole(User user, Role role);
    Task<ServiceResponse<GetUserDto?>> DeleteUserRole(User user, Role role);
}
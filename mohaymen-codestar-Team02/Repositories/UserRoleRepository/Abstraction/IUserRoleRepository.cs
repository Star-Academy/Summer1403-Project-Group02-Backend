using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.Auth;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.UserRoleRepository.Abstraction;

public interface IUserRoleRepository
{
    Task<UserRole> GetUserRole(long userId, long roleId);
    Task<IEnumerable<UserRole>> GetAllUserRoles(long id);
    Task<IEnumerable<UserRole>> GetUserRolesOfAUser(long userId);
    Task<IEnumerable<UserRole>> GetUserRolesOfARole(long roleId);
    Task<UserRole> AddUserRole(UserRole userRole);
    Task<UserRole> UpdateUserRole(UserRole userRole);
    Task DeleteUserRole(long userId, long roleId);
}
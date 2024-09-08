using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role> GetRole(long id);
    Task<Role> GetRole(string roleType);
    Task<bool> IsRoleMatching(IEnumerable<string> roles);
}
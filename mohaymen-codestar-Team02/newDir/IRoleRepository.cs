using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role> GetRole(long id);
}
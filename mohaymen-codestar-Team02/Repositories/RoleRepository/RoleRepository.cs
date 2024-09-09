using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.CleanArch1.Repositories.RoleRepository.Abstraction;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.Auth;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.RoleRepository;

public class RoleRepository : IRoleRepository
{
    private readonly IServiceProvider _serviceProvider;

    public RoleRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    
    public async Task<bool> IsRoleMatching(IEnumerable<string> roles)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        foreach (var role in roles)
        {
            var exists = await context.Roles.AnyAsync(dbRole => dbRole.RoleType.ToLower() == role.ToLower());
            if (!exists)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<IEnumerable<Role>> GetAllRoles()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.Roles.ToListAsync();
    }

    public async Task<Role> GetRole(long id)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.Roles.FirstOrDefaultAsync(u => u.RoleId == id);
    }
    
    public async Task<Role> GetRole(string roleType)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.Roles.FirstOrDefaultAsync(u => u.RoleType.ToLower() == roleType.ToLower());
    }

}
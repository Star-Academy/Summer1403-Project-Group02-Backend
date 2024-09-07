using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public class RoleRepository : IRoleRepository
{
    private readonly IServiceProvider _serviceProvider;

    public RoleRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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
}
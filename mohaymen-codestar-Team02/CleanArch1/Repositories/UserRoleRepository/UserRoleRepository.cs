using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public class UserRoleRepository : IUserRoleRepository
{

    private readonly IServiceProvider _serviceProvider;

    public UserRoleRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<UserRole> GetUserRole(long userId, long roleId)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.UserRoles.FirstOrDefaultAsync(ur=>ur.RoleId==roleId && ur.UserId==userId);
    }

    public async Task<IEnumerable<UserRole>> GetAllUserRoles(long id)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.UserRoles.ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetUserRolesOfAUser(long id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserRole>> GetUserRolesOfARole(long roleId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserRole> AddUserRole(UserRole userRole)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var result = await context.UserRoles.AddAsync(userRole);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public Task<UserRole> UpdateUserRole(UserRole userRole)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserRole(long userId, long roleId)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var result = await context.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == roleId && ur.UserId==userId);
        if (result != null)
        {
            context.UserRoles.Remove(result);
            await context.SaveChangesAsync();
        }
    }
}
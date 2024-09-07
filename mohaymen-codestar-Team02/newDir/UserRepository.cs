using Microsoft.EntityFrameworkCore;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public class UserRepository : IUserRepository
{

    private readonly IServiceProvider _serviceProvider;

    public UserRepository(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUser(long id)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User> AddUser(User user)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var result = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return result.Entity; 
    }
    
    public async Task<User?> UpdateUser(User user) //
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var result = await context.Users.FirstOrDefaultAsync(e => e.UserId == user.UserId);

        if (result != null)
        {
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.Email = user.Email;

            await context.SaveChangesAsync();

            return result;
        }

        return null;
    }

    public async Task DeleteUser(long id)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var result = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (result != null)
        {
            context.Users.Remove(result);
            await context.SaveChangesAsync();
        }
    }
}
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetUser(long id);
    Task<User> AddUser(User user);
    Task<User?> UpdateUser(User user);
    Task DeleteUser(long id);
}
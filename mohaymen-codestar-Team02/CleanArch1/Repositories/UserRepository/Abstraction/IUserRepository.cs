using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUserPaginated(int pageNumber);
    Task<User?> GetUserById(long id);
    Task<User?> GetUserByUsername(string username);
    Task<User> AddUser(User user);
    Task<User?> UpdateUser(User user);
    Task DeleteUser(long id);
    Task<bool> UserExists(string? username);
}
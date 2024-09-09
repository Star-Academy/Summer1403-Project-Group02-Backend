using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Models.Auth;

namespace mohaymen_codestar_Team02.CleanArch1.Repositories.UserRepository.Abstraction;

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
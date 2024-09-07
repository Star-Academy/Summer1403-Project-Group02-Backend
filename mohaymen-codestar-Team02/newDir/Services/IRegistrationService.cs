using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.newDir;

public interface IRegistrationService
{
    Task<ServiceResponse<GetUserDto?>> Register(User user, string password, List<Role> roles);
}
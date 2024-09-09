using mohaymen_codestar_Team02.CleanArch1.Dtos.AuthenticationDtos;
using mohaymen_codestar_Team02.Dto.Permission;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;

namespace mohaymen_codestar_Team02.CleanArch1.Services.AuthenticationService.Abstraction;

public interface IAuthenticateionService
{
    // User user, string password, List<Role> roles
    Task<ServiceResponse<GetUserDto?>> Register(RegisterDto registerDto);
    Task<ServiceResponse<GetUserDto?>> Login(LoginDto login);
    ServiceResponse<string?> Logout();
    Task<ServiceResponse<GetPermissionDto>> GetPermission();
    Task<ServiceResponse<string>> GetAuthorized();
}
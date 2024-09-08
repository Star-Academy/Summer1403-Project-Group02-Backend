using mohaymen_codestar_Team02.Dto.Permission;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using WebApplication15.Dtos;

namespace WebApplication15.Services;

public interface IAuthenticationService
{
    // User user, string password, List<Role> roles
    Task<ServiceResponse<GetUserDto?>> Register(RegisterDto registerDto);
    Task<ServiceResponse<GetUserDto?>> Login(LoginDto login);
    ServiceResponse<string?> Logout();
    Task<ServiceResponse<GetPermissionDto>> GetPermission();
    Task<ServiceResponse<string>> GetAuthorized();
}
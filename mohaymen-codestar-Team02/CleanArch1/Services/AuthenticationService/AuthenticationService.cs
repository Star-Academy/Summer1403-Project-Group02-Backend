using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.AuthenticationDtos;
using mohaymen_codestar_Team02.CleanArch1.Services.AuthenticationService.Abstraction;
using mohaymen_codestar_Team02.Dto.Permission;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.newDir;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;
using mohaymen_codestar_Team02.Services.PasswordHandller;
using WebApplication15.Dtos;

namespace mohaymen_codestar_Team02.CleanArch1.Services.AuthenticationService;

public class AuthenticationService : IAuthenticateionService
{

    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public AuthenticationService(IPasswordService passwordService, IUserRepository userRepository, ICookieService cookieService, ITokenService tokenService, IMapper mapper, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
    {
        _passwordService = passwordService;
        _userRepository = userRepository;
        _cookieService = cookieService;
        _tokenService = tokenService;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    /*
    public async Task<ServiceResponse<GetUserDto?>> Register(User user, string password, List<Role> roles)
    {
        var token = _cookieService.GetCookieValue();
            if (string.IsNullOrEmpty(token))
                return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);
            
            var adminId = _tokenService.GetUserId();
            var admin = await _userRepository.GetUserById(adminId);
            if (admin is null)
                return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);
            
            if (await _userRepository.GetUserByUsername(user.Username) is null)
                return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Conflict, Resources.UserAlreadyExistsMessage);

            if (!_passwordService.ValidatePassword(password))
                return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                    Resources.InvalidPasswordMessage);

            _passwordService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.Salt = passwordSalt;

            await _userRepository.AddUser(user);

            foreach (var role in roles)
                _userRoleRepository.AddUserRole(new UserRole
                {
                    RoleId = role.RoleId,
                    UserId = user.UserId
                });
        
            //var foundUser = await _userRepository.GetUser(user.UserId);
            var userDto = _mapper.Map<GetUserDto>(user);

            return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Created,
                Resources.UserCreatedSuccessfullyMessage);
    }

    public async Task<ServiceResponse<GetUserDto?>> Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.InvalidInputeMessage);

        var user = await _userRepository.GetUserByUsername(username);
        if (user is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        if (!_passwordService.VerifyPasswordHash(password, user.PasswordHash, user.Salt))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.WrongPasswordMessage);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString())
        };

        claims.AddRange(user.UserRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.RoleType)));

        _cookieService.CreateCookie(_tokenService.CreateToken(claims));

        var userDto = _mapper.Map<GetUserDto>(user);

        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success, Resources.LoginSuccessfulMessage);
    }

    public ServiceResponse<string?> Logout()
    {
        if (_cookieService.GetCookieValue() != null) _cookieService.GetExpiredCookie();

        return new ServiceResponse<string?>(null, ApiResponseType.Success,
            Resources.LogoutSuccessfulyMessage);
    }

    public async Task<ServiceResponse<GetPermissionDto>> GetPermission()
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetPermissionDto>(null, ApiResponseType.Unauthorized,
                Resources.UnauthorizedMessage);

        long userId = _tokenService.GetUserId();

        var user = await _userRepository.GetUserById(userId);
        if (user is null)
            return new ServiceResponse<GetPermissionDto>(null, ApiResponseType.BadRequest,
                Resources.UserNotFoundMessage);

        var roles = _tokenService.GetRolesFromToken();
        var splitRoles = roles?.Split(",");

        var permissions = await UnionPermissions(splitRoles);

        var permissionDto = new GetPermissionDto()
        {
            Permissions = permissions.ToList()
        };

        return new ServiceResponse<GetPermissionDto>(permissionDto, ApiResponseType.Success,
            Resources.GetPermissionsSuccessfulyMessage);    }

    public async Task<ServiceResponse<string>> GetAuthorized()
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<string>(null, ApiResponseType.Unauthorized,
                Resources.UnauthorizedMessage);

        var userId = _tokenService.GetUserId();

        var user = await _userRepository.GetUserById(userId);

        if (user is null)
            return new ServiceResponse<string>(null, ApiResponseType.BadRequest,
                Resources.UserNotFoundMessage);

        return new ServiceResponse<string>(user.Username, ApiResponseType.Success,
            Resources.AuthorizedMessage);
    }
    
    
    private async Task<HashSet<Permission>> UnionPermissions(string[]? splitRoles)
    {
        var permissions = new HashSet<Permission>();
        foreach (var userRole in splitRoles)
        {
            var role = await _roleRepository.GetRole(userRole);
            var permission = role?.Permissions;

            if (permission == null) continue;
            permissions.UnionWith(permission);
        }

        return permissions;
    }
    */

    public Task<ServiceResponse<GetUserDto?>> Register(RegisterDto registerDto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetUserDto?>> Login(LoginDto login)
    {
        throw new NotImplementedException();
    }

    public ServiceResponse<string?> Logout()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetPermissionDto>> GetPermission()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<string>> GetAuthorized()
    {
        throw new NotImplementedException();
    }
}
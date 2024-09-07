using AutoMapper;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;
using mohaymen_codestar_Team02.Services.PasswordHandller;

namespace mohaymen_codestar_Team02.newDir;

public class RegistrationService : IRegistrationService
{

    private readonly ICookieService _cookieService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;
    
    public RegistrationService(ICookieService cookieService, ITokenService tokenService, IUserRepository userRepository, IPasswordService passwordService, IUserRoleRepository userRoleRepository, IMapper mapper)
    {
        _cookieService = cookieService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordService = passwordService;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetUserDto?>> Register(User user, string password, List<Role> roles)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUser(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        if (await _userRepository.UserExists(user.Username))
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
    
}
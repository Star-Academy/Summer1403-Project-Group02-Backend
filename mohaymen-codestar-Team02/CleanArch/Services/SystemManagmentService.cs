using AutoMapper;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Dto.Role;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;

namespace mohaymen_codestar_Team02.newDir;

public class SystemManagmentService : ISystemManagmentService
{

    private readonly ICookieService _cookieService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;

    public SystemManagmentService(ICookieService cookieService, ITokenService tokenService, IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository)
    {
        _cookieService = cookieService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _mapper = mapper;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<ServiceResponse<IEnumerable<GetUserDto>?>> GetUsersPaginated(int pageNumber)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<IEnumerable<GetUserDto>?>(null, ApiResponseType.Unauthorized,
                Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await GetUser(adminId);
        if (admin is null)
            return new ServiceResponse<IEnumerable<GetUserDto>?>(null, ApiResponseType.BadRequest,
                Resources.UserNotFoundMessage);

        var users = await _userRepository.GetUserPaginated(pageNumber);

        var usersDto = users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
        return new ServiceResponse<IEnumerable<GetUserDto>?>(usersDto, ApiResponseType.Success,
            Resources.UserRetrievedMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> GetUser(string? username)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await GetUser(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var user = await GetUser(username);
        if (user is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var userDto = _mapper.Map<GetUserDto>(user);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success, Resources.UserRetrievedMassage);

    }

    public async Task<ServiceResponse<GetUserDto?>> GetUser(long? userId)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await GetUser(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var user = await GetUser(userId);
        if (user is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var userDto = _mapper.Map<GetUserDto>(user);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success, Resources.UserRetrievedMassage);

    }

    public async Task<ServiceResponse<GetUserDto?>> DeleteUser(User user)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await _userRepository.GetUserByUsername(user.Username);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        if (user.Username == admin.Username)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                Resources.CanNotDeleteYourselfMessage);

        await _userRepository.DeleteUser(foundUser.UserId); // todo
        
        var userDto = _mapper.Map<GetUserDto>(foundUser);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.UserDeletionSuccessfulMessage);
    }

    public async Task<ServiceResponse<GetUserDto?>> UpdateUser(User user)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await GetUser(user.Username);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        _userRepository.UpdateUser(user);
        
        var userDto = _mapper.Map<GetUserDto>(user);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.UserUpdateSuccessfulyMessage);
    }

    public async Task<ServiceResponse<IEnumerable<GetRoleDto>>> GetAllRoles()
    {
        var roles1 = await _roleRepository.GetAllRoles();
        var dtos = roles1.Select(r => _mapper.Map<GetRoleDto>(r));
        return new ServiceResponse<IEnumerable<GetRoleDto>>(dtos, ApiResponseType.Success, Resources.UsersRetrievedMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> AddUserRole(User user, Role role)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await _userRepository.GetUserByUsername(user.Username);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var foundRole = await _roleRepository.GetRole(role.RoleType);
        if (foundRole is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.RoleNotFoundMessage);

        if (await _userRoleRepository.GetUserRole(foundUser.UserId, foundRole.RoleId) is not null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                Resources.RoleAlreadyAssignedMessage);

        var userRole = new UserRole
        {
            RoleId = foundRole.RoleId,
            UserId = foundUser.UserId
        };

        await _userRoleRepository.AddUserRole(userRole);
        
        var userDto = _mapper.Map<GetUserDto>(foundUser);

        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.RoleAddedSuccessfulyMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> DeleteUserRole(User user, Role role)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(adminId);
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await _userRepository.GetUserByUsername(user.Username);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var foundRole = await _roleRepository.GetRole(role.RoleType);
        if (foundRole is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.RoleNotFoundMessage);


        var userRole = await _userRoleRepository.GetUserRole(foundUser.UserId, foundRole.RoleId);
        if (userRole is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                Resources.DontHaveThisRoleMessage);

        await _userRoleRepository.DeleteUserRole(userRole.UserId, userRole.RoleId);
        
        var userDto = _mapper.Map<GetUserDto>(foundUser);

        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.RoleRemovedSuccessfullyMessage);

    }
}
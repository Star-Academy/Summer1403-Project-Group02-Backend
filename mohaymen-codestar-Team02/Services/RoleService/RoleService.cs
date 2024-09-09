using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Repositories.RoleRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Repositories.UserRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Repositories.UserRoleRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Services.RoleService.Abstraction;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Dtos.Extra.RoleDto;
using mohaymen_codestar_Team02.Dtos.Extra.UserDto;
using mohaymen_codestar_Team02.Models.Auth;

namespace mohaymen_codestar_Team02.CleanArch1.Services.RoleService;

public class RoleService : IRoleService
{
    private IUserRoleRepository _userRoleRepository;
    private IRoleRepository _roleRepository;
    private IMapper _mapper;
    private ICookieService _cookieService;
    private IUserRepository _userRepository;
    private ITokenService _tokenService;

    public RoleService(IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IMapper mapper, ICookieService cookieService, IUserRepository userRepository, ITokenService tokenService)
    {
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _cookieService = cookieService;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<ServiceResponse<IEnumerable<GetRoleDto>>> GetAllRoles()
    {
        var roles1 = await _roleRepository.GetAllRoles();
        var dtos = roles1.Select(r => _mapper.Map<GetRoleDto>(r));
        return new ServiceResponse<IEnumerable<GetRoleDto>>(dtos, ApiResponseType.Success, Resources.UsersRetrievedMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> AddUserRole(long userId, long roleId)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(long.Parse(adminId));
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await _userRepository.GetUserById(userId);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var foundRole = await _roleRepository.GetRole(roleId);
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
        
        // role list is not up to date
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.RoleAddedSuccessfulyMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> DeleteUserRole(long userId, long roleId)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(long.Parse(adminId));
        if (admin is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var foundUser = await _userRepository.GetUserById(userId);
        if (foundUser is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var foundRole = await _roleRepository.GetRole(roleId);
        if (foundRole is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest, Resources.RoleNotFoundMessage);


        var userRole = await _userRoleRepository.GetUserRole(foundUser.UserId, foundRole.RoleId);
        if (userRole is null)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                Resources.DontHaveThisRoleMessage);

        await _userRoleRepository.DeleteUserRole(userRole.UserId, userRole.RoleId);

        var userDto = _mapper.Map<GetUserDto>(foundUser);

        // role list is not up to date
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.RoleRemovedSuccessfullyMessage);

    }
}
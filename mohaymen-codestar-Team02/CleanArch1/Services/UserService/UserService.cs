using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.UserDtos;
using mohaymen_codestar_Team02.CleanArch1.Repositories.UserRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Services.UserService.Abstraction;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using UpdateUserDto = mohaymen_codestar_Team02.CleanArch1.Dtos.UserDtos.UpdateUserDto;
using mohaymen_codestar_Team02.Data;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;
using mohaymen_codestar_Team02.Services.PasswordHandller;

namespace mohaymen_codestar_Team02.CleanArch1.Services.UserService;

public class UserService : IUserService
{
    private readonly ICookieService _cookieService;
    private ITokenService _tokenService;
    private IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private IPasswordService _passwordService;

    public UserService(ICookieService cookieService, ITokenService tokenService, IUserRepository userRepository,
        IMapper mapper, IPasswordService passwordService)
    {
        _cookieService = cookieService;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    public async Task<ServiceResponse<List<GetUserDto>?>> GetUsersPaginated(int pageNumber)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<List<GetUserDto>?>(null, ApiResponseType.Unauthorized,
                Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(long.Parse(adminId));
        if (admin is null)
            return new ServiceResponse<List<GetUserDto>?>(null, ApiResponseType.BadRequest,
                Resources.UserNotFoundMessage);

        var users = await _userRepository.GetUserPaginated(pageNumber);

        var usersDto = users.Select(u => _mapper.Map<GetUserDto>(u)).ToList();
        return new ServiceResponse<List<GetUserDto>?>(usersDto, ApiResponseType.Success,
            Resources.UserRetrievedMassage);
    }

    public async Task<ServiceResponse<GetUserDto>> GetSingleUser(long userId)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var adminId = _tokenService.GetUserId();
        var admin = await _userRepository.GetUserById(long.Parse(adminId));
        if (admin is null)
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        var user = await _userRepository.GetUserById(userId);
        if (user is null)
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.NotFound, Resources.UserNotFoundMessage);

        var userDto = _mapper.Map<GetUserDto>(user);
        return new ServiceResponse<GetUserDto>(userDto, ApiResponseType.Success, Resources.UserRetrievedMassage);
    }

    public async Task<ServiceResponse<GetUserDto?>> DeleteUser(long userId)
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

        if (foundUser.Username == admin.Username)
            return new ServiceResponse<GetUserDto?>(null, ApiResponseType.BadRequest,
                Resources.CanNotDeleteYourselfMessage);

        await _userRepository.DeleteUser(foundUser.UserId); // todo

        var userDto = _mapper.Map<GetUserDto>(foundUser);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.UserDeletionSuccessfulMessage);
    }

    public async Task<ServiceResponse<GetUserDto?>> UpdateUser(UpdateUserDto updateUserDto, long userId)
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

        var user = new User()
        {
            Username = foundUser.Username,
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            Email = updateUserDto.Email,
            UserRoles = foundUser.UserRoles
        };
        await _userRepository.UpdateUser(user);

        var userDto = _mapper.Map<GetUserDto>(user);
        return new ServiceResponse<GetUserDto?>(userDto, ApiResponseType.Success,
            Resources.UserUpdateSuccessfulyMessage);
    }

    public async Task<ServiceResponse<GetUserDto>> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var token = _cookieService.GetCookieValue();
        if (string.IsNullOrEmpty(token))
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.Unauthorized, Resources.UnauthorizedMessage);

        var userId = _tokenService.GetUserId();
        var user = await _userRepository.GetUserById(long.Parse(userId));

        if (user is null)
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.BadRequest, Resources.UserNotFoundMessage);

        if (!_passwordService.VerifyPasswordHash(changePasswordDto.PreviousPassword, user.PasswordHash, user.Salt))
            return new ServiceResponse<GetUserDto>(null, ApiResponseType.BadRequest, Resources.WrongPasswordMessage);

        _passwordService.CreatePasswordHash(changePasswordDto.NewPassword, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.Salt = passwordSalt;

        await _userRepository.UpdateUser(user);

        return new ServiceResponse<GetUserDto>(null, ApiResponseType.Success,
            Resources.PasswordChangedSuccessfulyMessage);
    }
}
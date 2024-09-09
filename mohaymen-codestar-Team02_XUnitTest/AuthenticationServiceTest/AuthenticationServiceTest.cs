using System.Security.Claims;
using System.Text;
using AutoMapper;
using mohaymen_codestar_Team02.CleanArch1.Dtos.AuthenticationDtos;
using mohaymen_codestar_Team02.CleanArch1.Repositories.RoleRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Repositories.UserRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Repositories.UserRoleRepository.Abstraction;
using mohaymen_codestar_Team02.CleanArch1.Services.AuthenticationService;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using mohaymen_codestar_Team02.Services;
using mohaymen_codestar_Team02.Services.CookieService;
using mohaymen_codestar_Team02.Services.PasswordHandller;
using NSubstitute;

namespace mohaymen_codestar_Team02_XUnitTest.CleanArcServices;

public class AuthenticationServiceTests1
{
    private readonly AuthenticationService _sut;
    private readonly ITokenService _tokenService;
    private readonly ICookieService _cookieService;
    private readonly IPasswordService _passwordService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public AuthenticationServiceTests1()
    {
        _passwordService = Substitute.For<IPasswordService>();
        _cookieService = Substitute.For<ICookieService>();
        _tokenService = Substitute.For<ITokenService>();

        _userRepository = Substitute.For<IUserRepository>();
        _roleRepository = Substitute.For<IRoleRepository>();
        _userRoleRepository = Substitute.For<IUserRoleRepository>();

        var config = new MapperConfiguration(cfg => { cfg.CreateMap<User, GetUserDto>(); });
        var mapper = config.CreateMapper();

        _sut = new AuthenticationService( _passwordService, _cookieService, _tokenService, mapper,
            _userRepository, _roleRepository, _userRoleRepository);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Login_ShouldReturnBadRequest_WhenUserIsNullOrEmpty(string username)
    {
        // Arrange
        var password = "password123";
        LoginDto testLoginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _sut.Login(testLoginDto);

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Login_ShouldReturnBadRequest_WhenPasswordIsNullOrEmpty(string password)
    {
        // Arrange
        var username = "username123";
        LoginDto testLoginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _sut.Login(testLoginDto);

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        var username = "nonexistentUser";
        var password = "password123";
        LoginDto testLoginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _sut.Login(testLoginDto);

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenPasswordIsIncorrect()
    {
        // Arrange
        var username = "existingUser";
        var password = "wrongPassword";
        MockRepository(username, "correctPassword");

        _passwordService.VerifyPasswordHash(password, Arg.Any<byte[]>(), Arg.Any<byte[]>())
            .Returns(false);

        LoginDto testLoginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _sut.Login(testLoginDto);

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Fact]
    public async Task Login_ShouldReturnSuccess_WhenCredentialsAreCorrect()
    {
        // Arrange
        var username = "existingUser";
        var password = "correctPassword";
        MockRepository(username, password);
        var fakeToken = "fakeToken";
        _tokenService.CreateToken(Arg.Any<List<Claim>>()).Returns(fakeToken);
        _passwordService.VerifyPasswordHash(password, Arg.Any<byte[]>(), Arg.Any<byte[]>())
            .Returns(true);
        LoginDto testLoginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };

        // Act
        var result = await _sut.Login(testLoginDto);

        // Assert
        Assert.Equal(ApiResponseType.Success, result.Type);
    }

    private void MockRepository(string username, string password)
    {
        var user = new User
        {
            Username = username,
            Salt = new byte[128],
            PasswordHash = new byte[64]
        };
        using (var hmac = new System.Security.Cryptography.HMACSHA512(user.Salt))
        {
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        _userRepository.GetUserByUsername(username).Returns(user);
    }

    [Fact]
    public void Logout_ShouldCallGetExpiredCookie_WhenCookieIsPresent()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("someCookieValue");
        _cookieService.GetExpiredCookie();

        // Act
        var result = _sut.Logout();

        // Assert
        Assert.Equal(ApiResponseType.Success, result.Type);
    }

    [Fact]
    public void Logout_ShouldNotCallGetExpiredCookie_WhenCookieIsNotPresent()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns((string?)null);
        _cookieService.DidNotReceive().GetExpiredCookie();
        // Act
        var result = _sut.Logout();

        // Assert
        Assert.Equal(ApiResponseType.Success, result.Type);
    }

    [Fact]
    public async Task GetPermission_ShouldReturnUnauthorized_WhenTokenIsEmpty()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns(string.Empty);

        // Act
        var result = await _sut.GetPermission();

        // Assert
        Assert.Equal(ApiResponseType.Unauthorized, result.Type);
    }

    [Fact]
    public async Task GetPermission_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("fakeCookie");
        var userId = "1";
        _tokenService.GetUserId().Returns(userId);
        _userRepository.GetUserById(Arg.Any<long>()).Returns((User?)null);
        
        // Act
        var result = await _sut.GetPermission();

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Fact]
    public async Task GetPermission_ShouldReturnSuccess_WhenUserExist()
    { 
        // Arrange
        _cookieService.GetCookieValue().Returns("fakeCookie");
        var userId = "1";
        var user = new User
        {
            UserId = 1,
            Username = "existingUser"
        };
        _tokenService.GetUserId().Returns(userId);
        _userRepository.GetUserById(Arg.Any<long>()).Returns(user);
        List<string> roleType = new List<string>() { "SystemAdmin" };
        var systemAdminPermission = new List<Permission>
        {
            Permission.Login,
            Permission.Logout,
            Permission.UserRegister
        };
        MockUserWithRole("admin", roleType, 3, new List<List<Permission>>(){ systemAdminPermission});

        // Act
        var result = await _sut.GetPermission();

        // Assert
        Assert.Equal(ApiResponseType.Success, result.Type);
    }


    [Fact]
    public async Task GetPermission_ShouldReturnUnionPermissions_WhenUserExist()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("fakeCookie");
        var userId = "1";
        var user = new User
        {
            UserId = 1,
            Username = "existingUser"
        };
        _tokenService.GetUserId().Returns(userId);
        _userRepository.GetUserById(Arg.Any<long>()).Returns(user);
        var systemAdminPermission = new List<Permission>
        {
            Permission.Login,
            Permission.Logout,
            Permission.UserRegister
        };
        var dataAdminPermission = new List<Permission>
        {
            Permission.Login,
            Permission.Logout
        };
        List<string> roleType = new List<string>() { "SystemAdmin", "DataAdmin" };
        List<List<Permission>> permission = new List<List<Permission>>()
        {
            systemAdminPermission,
            dataAdminPermission
        };
        MockUserWithRole("admin",roleType , 4, permission);
        
        _tokenService.GetRolesFromToken().Returns("SystemAdmin,DataAdmin");

        var expected = new List<Permission>
        {
            Permission.Login,
            Permission.Logout,
            Permission.UserRegister
        };

        // Act
        var result = await _sut.GetPermission();

        // Assert
        Assert.Equal(expected.Count, result.Data?.Permissions.Count);
    }

    private void MockUserWithRole(string userName, List<string> roleType, long id, List<List<Permission>> permissions)
    {
        for (int i = 0; i < roleType.Count; i++)
        {
            var role = new Role { RoleType = roleType[i], RoleId = id, Permissions = permissions[i] };
            var user = new User
            {
                Salt = Array.Empty<byte>(),
                PasswordHash = Array.Empty<byte>(),
                Username = userName,
                UserId = id,
                UserRoles = new List<UserRole>()
                {
                    new UserRole { UserId = id, RoleId = role.RoleId }
                }
            };
            _userRepository.GetUserById(id).Returns(user);
            _roleRepository.GetRole(roleType[i]).Returns(new Role
            {
                RoleType = "SystemAdmin",
                Permissions = permissions[i]
            });
        }
    }
    
     [Fact]
    public async Task Register_ShouldReturnUnauthorized_WhenTokenIsEmpty()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns(string.Empty);
        RegisterDto registerDto = new RegisterDto()
        {
            Username = "testUsername",
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email"
        };

        // Act
        var result = await _sut.Register(registerDto);

        // Assert
        Assert.Equal(ApiResponseType.Unauthorized, result.Type);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnBadRequest_WhenAdminNotFound()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("cookieValue");
        RegisterDto registerDto = new RegisterDto()
        {
            Username = "testUsername",
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email"
        };
        var userId = "1";
        var user = new User
        {
            UserId = 1,
            Username = "existingAdminUser"
        };
        _tokenService.GetUserId().Returns(userId);
        _userRepository.GetUserById(Arg.Any<long>()).Returns(user);
        
        // Act
        var response = await _sut.Register(registerDto);

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, response.Type);
    }
    
    [Fact]
    public async Task Register_ShouldReturnConflict_WhenUserAlreadyExists()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("cookieValue");
        RegisterDto registerDto = new RegisterDto()
        {
            Username = "existingUser",
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email"
        };
        var adminId = "1";
        var admin = new User
        {
            UserId = 1,
            Username = "existingAdmin"
        };
        var userId = "2";
        var user = new User
        {
            UserId = 2,
            Username = "existingUser"
        };
        _tokenService.GetUserId().Returns(adminId);
        _userRepository.GetUserByUsername(registerDto.Username).Returns(new User());
        _userRepository.GetUserById(long.Parse(adminId)).Returns(admin);
        _userRepository.GetUserById(long.Parse(userId)).Returns(user);
        
        // Act
        var result = await _sut.Register(registerDto);

        // Assert
        Assert.Equal(ApiResponseType.Conflict, result.Type);
    }
    
    
    [Fact]
    public async Task Register_ShouldReturnCreated_WhenUserIsSuccessfullyRegistered()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns("cookieValue");
        RegisterDto registerDto = new RegisterDto()
        {
            Username = "registerUser",
            FirstName = "firstName",
            LastName = "lastName",
            Email = "email",
            RolesId = new List<int>(){1 , 2}
        };

        var adminId = "1";
        var admin = new User
        {
            UserId = 1,
            Username = "existingAdmin"
        };
        _tokenService.GetUserId().Returns(adminId);
        _userRepository.GetUserById(long.Parse(adminId)).Returns(admin);

        var fakePasswordHash = new byte[] { 1, 2, 3, 4 };
        var fakePasswordSalt = new byte[] { 5, 6, 7, 8 };
        
        _passwordService
            .When(x => x.CreatePasswordHash(Arg.Any<string>(), out Arg.Any<byte[]>(), out Arg.Any<byte[]>()))
            .Do(x =>
            {
                x[1] = fakePasswordHash;
                x[2] = fakePasswordSalt;
            });

        _passwordService.ValidatePassword(Arg.Any<string>()).Returns(true);

        _userRepository.GetUserByUsername(registerDto.Username)
            .Returns((User?)null,
                new User()
                {
                    Username = "registerUser",
                    FirstName = "firstName",
                    LastName = "lastName",
                    Email = "email"
                });
    
        var role1 = new Role { RoleId = 1, RoleType = "SystemAdmin" };
        _roleRepository.GetRole(1).Returns(role1);
        var userRole1 = new UserRole { RoleId = 1, UserId = 1 };
        _userRoleRepository.GetUserRole(1, 1).Returns(userRole1);
        
        var role2 = new Role { RoleId = 2, RoleType = "DataAdmin" };
        _roleRepository.GetRole(2).Returns(role2);
        var userRole2 = new UserRole { RoleId = 2, UserId = 1 };
        _userRoleRepository.GetUserRole(2, 1).Returns(userRole2);

        // Act
        var result = await _sut.Register(registerDto);

        // Assert
        Assert.Equal(ApiResponseType.Created, result.Type);
    }
    
    [Fact]
    public async Task GetAuthorized_ShouldReturnUnauthorized_WhenCookieIsNullOrEmpty()
    {
        // Arrange
        _cookieService.GetCookieValue().Returns((string?)null);

        // Act
        var result = await _sut.GetAuthorized();

        // Assert
        Assert.Equal(ApiResponseType.Unauthorized, result.Type);
    }

    [Fact]
    public async Task GetAuthorized_ShouldReturnBadRequest_WhenUserNotFound()
    {
        // Arrange
        var fakeUserId = "1";
        _cookieService.GetCookieValue().Returns("fakeToken");
        _tokenService.GetUserId().Returns(fakeUserId);

        _userRepository.GetUserById(long.Parse(fakeUserId)).Returns((User?)null);

        // Act
        var result = await _sut.GetAuthorized();

        // Assert
        Assert.Equal(ApiResponseType.BadRequest, result.Type);
    }

    [Fact]
    public async Task GetAuthorized_ShouldReturnSuccess_WhenUserIsFound()
    {
        // Arrange
        var fakeUserId = "1";
        var user = new User { UserId = 1, Username = "testUser" };
    
        _cookieService.GetCookieValue().Returns("fakeToken");
        _tokenService.GetUserId().Returns(fakeUserId);

        _userRepository.GetUserById(long.Parse(fakeUserId)).Returns(user);

        // Act
        var result = await _sut.GetAuthorized();

        // Assert
        Assert.Equal(ApiResponseType.Success, result.Type);
        Assert.Equal(user.Username, result.Data);
    }
}
using mohaymen_codestar_Team02.Dtos.Extra.UserDto;
using mohaymen_codestar_Team02.Dtos.UserDtos;
using mohaymen_codestar_Team02.Models;
using UpdateUserDto = mohaymen_codestar_Team02.Dtos.UserDtos.UpdateUserDto;

namespace mohaymen_codestar_Team02.CleanArch1.Services.UserService.Abstraction;

public interface IUserService
{
    Task<ServiceResponse<List<GetUserDto>?>> GetUsersPaginated(int pageNumber);
    Task<ServiceResponse<GetUserDto>> GetSingleUser(long userId);
    Task<ServiceResponse<GetUserDto?>> DeleteUser(long userId);
    Task<ServiceResponse<GetUserDto?>> UpdateUser(UpdateUserDto updateUserDto, long userId);
    Task<ServiceResponse<GetUserDto>> ChangePassword(ChangePasswordDto changePasswordDto);
}
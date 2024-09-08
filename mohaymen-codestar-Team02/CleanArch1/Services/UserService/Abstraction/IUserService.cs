using mohaymen_codestar_Team02.CleanArch1.Dtos.UserDtos;
using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using UpdateUserDto = mohaymen_codestar_Team02.CleanArch1.Dtos.UserDtos.UpdateUserDto;

namespace mohaymen_codestar_Team02.CleanArch1.Services.UserService.Abstraction;

public interface IUserService
{
    Task<ServiceResponse<List<GetUserDto>>> GetUsersPaginated(int pageNumber);
    Task<ServiceResponse<GetUserDto>> GetSingleUser(long userId);
    Task<ServiceResponse<GetUserDto?>> DeleteUser(long userId);
    Task<ServiceResponse<GetUserDto?>> UpdateUser(UpdateUserDto updateUserDto);
    Task<ServiceResponse<GetUserDto>> ChangePassword(ChangePasswordDto changePasswordDto);
}
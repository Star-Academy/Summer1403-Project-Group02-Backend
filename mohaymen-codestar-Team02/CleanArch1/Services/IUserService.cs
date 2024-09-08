using mohaymen_codestar_Team02.Dto.User;
using mohaymen_codestar_Team02.Models;
using WebApplication15.Dtos.UserDtos;
using UpdateUserDto = WebApplication15.Dtos.UserDtos.UpdateUserDto;

namespace WebApplication15.Services;

public interface IUserService
{
    Task<ServiceResponse<List<GetUserDto>>> GetUsersPaginated(int pageNumber);
    Task<ServiceResponse<GetUserDto>> GetSingleUser(long userId);
    Task<ServiceResponse<GetUserDto?>> DeleteUser(long userId);
    Task<ServiceResponse<GetUserDto?>> UpdateUser(UpdateUserDto updateUserDto);
    Task<ServiceResponse<GetUserDto>> ChangePassword(ChangePasswordDto changePasswordDto);
}
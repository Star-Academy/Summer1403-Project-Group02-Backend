using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.Dtos.Extra.UserDto;

public class ChangePasswordUserDto
{
    [Required] public string PreviousPassword { get; init; }
    [Required] public string NewPassword { get; init; }
}
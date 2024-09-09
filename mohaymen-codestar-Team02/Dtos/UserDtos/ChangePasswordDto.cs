using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.Dtos.UserDtos;

public class ChangePasswordDto
{
    [Required] public string PreviousPassword { get; init; }
    [Required] public string NewPassword { get; init; }
}
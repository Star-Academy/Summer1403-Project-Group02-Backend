using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Dtos.UserDtos;

public class ChangePasswordDto
{
    [Required] public string PreviousPassword { get; init; }
    [Required] public string NewPassword { get; init; }
}
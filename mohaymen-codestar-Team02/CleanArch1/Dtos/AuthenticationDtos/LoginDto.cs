using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.CleanArch1.Dtos.AuthenticationDtos;

public class LoginDto
{
    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
}
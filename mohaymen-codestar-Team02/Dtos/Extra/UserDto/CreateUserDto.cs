using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.Dtos.Extra.UserDto;

public class CreateUserDto
{
    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
    [Required] public string FirstName { get; init; }
    [Required] public string LastName { get; init; }
    [Required] public string Email { get; init; }
    [Required] public List<string> Roles { get; init; }
}
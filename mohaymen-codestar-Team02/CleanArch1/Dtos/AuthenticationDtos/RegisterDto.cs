using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Dtos;

public class RegisterDto
{
    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
    [Required] public string FirstName { get; init; }
    [Required] public string LastName { get; init; }
    [Required] public string Email { get; init; }
    [Required] public List<string> Roles { get; init; }  // IEnumerable<>
}
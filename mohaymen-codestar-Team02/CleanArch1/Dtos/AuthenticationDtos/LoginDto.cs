using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Dtos;

public class LoginDto
{
    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
}
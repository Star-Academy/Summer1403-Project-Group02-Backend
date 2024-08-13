using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.Models;

public class User
{
    public long UserId { get; init; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    //dont add normal pass
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public HashSet<UserRole> UserRoles { get; set; } = new();
}
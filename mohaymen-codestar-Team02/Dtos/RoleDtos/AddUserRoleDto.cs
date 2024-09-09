using System.ComponentModel.DataAnnotations;

namespace mohaymen_codestar_Team02.CleanArch1.Dtos.RoleDtos;

public class AddUserRoleDto
{
    [Required] public string RoleType { get; init; }
}
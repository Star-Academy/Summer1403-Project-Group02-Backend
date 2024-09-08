using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Dtos.RoleDtos;

public class AddUserRoleDto
{
    [Required] public string RoleType { get; init; }
}
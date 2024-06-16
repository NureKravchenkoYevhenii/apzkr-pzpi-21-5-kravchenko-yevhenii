using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parky.Infrastructure.Models;

public class SetUserRoleModel
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public Role Role { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models;
public class ResetPasswordModel
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Token { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    [Compare("Password", ErrorMessage = "PASSWORD_MATCH_ERROR")]
    public string ConfirmPassword { get; set; } = null!;
}

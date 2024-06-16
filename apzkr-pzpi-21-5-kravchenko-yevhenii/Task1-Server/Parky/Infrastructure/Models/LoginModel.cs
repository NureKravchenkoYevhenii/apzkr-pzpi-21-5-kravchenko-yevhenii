using System.ComponentModel.DataAnnotations;

namespace Parky.Infrastructure.Models;

public class LoginModel
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    //[MinLength(ValidationConstant.LoginMinLength, ErrorMessage = "MIN_LENGTH_ERROR")]
    //[MaxLength(ValidationConstant.LoginMaxLength, ErrorMessage = "MAX_LENGTH_ERROR")]
    //[RegularExpression(RegularExpressions.Login,
    //    ErrorMessage = "INVALID_LOGIN_ERROR")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    //[MinLength(ValidationConstant.PasswordMinLength, ErrorMessage = "MIN_LENGTH_ERROR")]
    //[MaxLength(ValidationConstant.PasswordMaxLength, ErrorMessage = "MAX_LENGTH_ERROR")]
    //[RegularExpression(RegularExpressions.Password,
    //    ErrorMessage = "INVALID_PASSWORD_ERROR")]
    public string Password { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace Parky.Infrastructure.Models;

public class ForgotPasswordModel
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    [EmailAddress(ErrorMessage = "INVALID_EMAIL_ADDRESS_ERROR")]
    //[MinLength(ValidationConstant.EmailMinLength, ErrorMessage = "MIN_LENGTH_ERROR")]
    public string Email { get; set; } = null!;
}

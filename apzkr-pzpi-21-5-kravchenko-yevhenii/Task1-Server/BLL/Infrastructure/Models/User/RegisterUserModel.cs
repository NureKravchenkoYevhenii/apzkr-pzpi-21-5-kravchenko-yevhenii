using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models;
public class RegisterUserModel
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Login { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string LastName { get; set; } = null!;

    public byte[]? ProfilePicture { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string PhoneNumber { get; set; } = null!;

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public DateTime BirthDate { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Email { get; set; } = null!;
}

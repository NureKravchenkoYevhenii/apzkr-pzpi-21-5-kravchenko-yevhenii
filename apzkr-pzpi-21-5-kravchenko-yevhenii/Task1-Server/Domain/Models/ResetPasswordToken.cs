using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class ResetPasswordToken : BaseEntity
{
    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public string Token { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public DateTime ExpiresOnUtc { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    public int UserId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    #endregion
}

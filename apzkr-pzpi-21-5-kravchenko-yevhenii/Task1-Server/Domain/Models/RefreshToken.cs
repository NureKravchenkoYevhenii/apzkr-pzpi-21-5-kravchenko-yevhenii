using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class RefreshToken : BaseEntity
{
    [Required]
    public Guid Token { get; set; }

    [Required]
    public DateTime ExpiresOnUtc { get; set; }

    [Required]
    public int UserId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; } = null!;

    #endregion
}

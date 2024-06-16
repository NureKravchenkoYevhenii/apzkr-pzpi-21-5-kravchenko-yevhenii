using Infrastructure.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class User : BaseEntity
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string PasswordSalt { get; set; }

    [Required]
    public DateTime RegistrationDate { get; set; }

    [Required]
    public Role Role { get; set; }

    public bool IsBlocked { get; set; }

    #region Relations

    [JsonIgnore]
    public UserProfile UserProfile { get; set; }

    [JsonIgnore]
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    [JsonIgnore]
    public ICollection<ResetPasswordToken> ResetPasswordTokens { get; set; }

    [JsonIgnore]
    public ICollection<Payment> Payments { get; set; }

    [JsonIgnore]
    public ICollection<ParkingSession> ParkingSessions { get; set; }

    [JsonIgnore]
    public ICollection<Booking> Bookings { get; set; }

    [JsonIgnore]
    public ICollection<UserMembership> UserMemberships { get; set; }

    [JsonIgnore]
    public ICollection<Car> Cars { get; set; }

    #endregion
}

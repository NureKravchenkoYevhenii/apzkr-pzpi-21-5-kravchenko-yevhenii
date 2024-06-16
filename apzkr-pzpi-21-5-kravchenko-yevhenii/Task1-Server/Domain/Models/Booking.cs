using System.Text.Json.Serialization;

namespace Domain.Models;
public class Booking : BaseEntity
{
    public DateTime BookDate { get; set; }

    public TimeSpan ExpiresIn { get; set; }

    public int UserId { get; set; }

    public int ParkingPlaceId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public ParkingPlace ParkingPlace { get; set; }

    #endregion
}

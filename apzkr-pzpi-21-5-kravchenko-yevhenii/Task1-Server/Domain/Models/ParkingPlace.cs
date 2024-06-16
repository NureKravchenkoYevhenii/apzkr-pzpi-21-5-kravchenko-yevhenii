using System.Text.Json.Serialization;

namespace Domain.Models;
public class ParkingPlace : BaseEntity
{
    public string Name { get; set; }

    #region Relations

    [JsonIgnore]
    public ICollection<ParkingSession> ParkingSessions { get; set; }

    [JsonIgnore]
    public ICollection<Booking> Bookings { get; set; }

    #endregion
}

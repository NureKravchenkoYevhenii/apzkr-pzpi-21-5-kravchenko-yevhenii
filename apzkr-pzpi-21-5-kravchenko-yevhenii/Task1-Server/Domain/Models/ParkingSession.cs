using System.Text.Json.Serialization;

namespace Domain.Models;
public class ParkingSession : BaseEntity
{
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int UserId { get; set; }

    public int ParkingPlaceId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public ParkingPlace ParkingPlace { get; set; }

    #endregion
}

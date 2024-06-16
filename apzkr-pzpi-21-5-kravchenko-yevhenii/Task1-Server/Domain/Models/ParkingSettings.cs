namespace Domain.Models;
public class ParkingSettings : BaseEntity
{
    public int BookingTimeAdvanceInMinutes { get; set; }

    public int BookingDurationInMinutes { get; set; }
}

namespace BLL.Infrastructure.Models.Booking;
public class BookingModel
{
    public int Id { get; set; }

    public string BookDate { get; set; } = null!;

    public string ExpireDate { get; set; } = null!;

    public string ParkingPlaceName { get; set; } = null!;
}

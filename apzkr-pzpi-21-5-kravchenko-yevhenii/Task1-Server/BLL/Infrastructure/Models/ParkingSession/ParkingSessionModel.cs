namespace BLL.Infrastructure.Models.ParkingSession;
public class ParkingSessionModel
{
    public int Id { get; set; }

    public string StartDate { get; set; } = null!;

    public string? EndDate { get; set; }

    public int UserId { get; set; }

    public string ParkingPlaceName { get; set; } = null!;
}

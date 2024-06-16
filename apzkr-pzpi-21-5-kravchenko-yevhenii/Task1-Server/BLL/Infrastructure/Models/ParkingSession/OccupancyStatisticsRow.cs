namespace BLL.Infrastructure.Models.ParkingSession;
public class OccupancyStatisticsRow
{
    public DateTime Date { get; set; }

    public int Hour { get; set; }

    public int OccupiedPlacesAmount { get; set; }
}

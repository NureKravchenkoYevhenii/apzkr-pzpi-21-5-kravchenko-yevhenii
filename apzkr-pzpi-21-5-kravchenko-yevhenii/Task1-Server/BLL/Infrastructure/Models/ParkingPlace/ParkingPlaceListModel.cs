namespace BLL.Infrastructure.Models.ParkingPlace;
public class ParkingPlaceListModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsOccupied { get; set; }
}

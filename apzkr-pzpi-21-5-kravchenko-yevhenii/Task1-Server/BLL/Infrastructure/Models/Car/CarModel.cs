namespace BLL.Infrastructure.Models.Car;
public class CarModel
{
    public int Id { get; set; }

    public string CarNumber { get; set; } = null!;

    public int UserId { get; set; }
}

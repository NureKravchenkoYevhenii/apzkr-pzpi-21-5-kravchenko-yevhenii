namespace BLL.Infrastructure.Models.Booking;
public class CreateBookingModel
{
    public int Id { get; set; }

    public DateTime BookDate { get; set; }

    public int UserId { get; set; }
}

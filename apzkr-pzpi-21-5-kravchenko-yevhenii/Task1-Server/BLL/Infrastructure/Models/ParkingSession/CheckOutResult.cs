namespace BLL.Infrastructure.Models.ParkingSession;
public class CheckOutResult
{
    public bool IsPaid { get; set; }

    public string? PaymentUrl { get; set; }
}

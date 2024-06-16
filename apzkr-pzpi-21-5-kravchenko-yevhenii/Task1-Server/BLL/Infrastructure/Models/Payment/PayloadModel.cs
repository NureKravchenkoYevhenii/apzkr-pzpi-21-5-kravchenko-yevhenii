using Infrastructure.Enums;

namespace BLL.Infrastructure.Models.Payment;
public class PayloadModel
{
    public PurchaseType PurchaseType { get; set; }

    public int PayItemId { get; set; }

    public int? UserId { get; set; }
}

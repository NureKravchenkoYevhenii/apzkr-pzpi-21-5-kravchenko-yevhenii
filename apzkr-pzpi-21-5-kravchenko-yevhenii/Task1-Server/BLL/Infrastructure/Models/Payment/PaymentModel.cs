using Infrastructure.Enums;

namespace BLL.Infrastructure.Models.Payment;
public class PaymentModel
{
    public int Id { get; set; }

    public string Transaction { get; set; } = null!;

    public decimal Sum { get; set; }

    public DateTime? PayDate { get; set; }

    public string PayDateStr { get; set; } = null!;

    public string Description { get; set; } = null!;

    public PurchaseType PurchaseType { get; set; }

    public int? UserId { get; set; }

    public string? UserName { get; set; }
}

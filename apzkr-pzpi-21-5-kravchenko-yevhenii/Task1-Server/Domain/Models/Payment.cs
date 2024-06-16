using Infrastructure.Enums;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class Payment : BaseEntity
{
    public string Transaction { get; set; }

    public decimal Sum { get; set; }

    public DateTime? PayDate { get; set; }

    public string Description { get; set; }

    public PurchaseType PurchaseType { get; set; }

    public int? UserId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    #endregion
}

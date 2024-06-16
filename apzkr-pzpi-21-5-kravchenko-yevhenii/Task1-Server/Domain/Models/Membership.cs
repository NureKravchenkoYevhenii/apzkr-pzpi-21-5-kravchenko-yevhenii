using System.Text.Json.Serialization;

namespace Domain.Models;
public class Membership : BaseEntity
{
    public string Name { get; set; }

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }

    #region Relations

    [JsonIgnore]
    public ICollection<UserMembership> UserMemberships { get; set; }

    #endregion
}

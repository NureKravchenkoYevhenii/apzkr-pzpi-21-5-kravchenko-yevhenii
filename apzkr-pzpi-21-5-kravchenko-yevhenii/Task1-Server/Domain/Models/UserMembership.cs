using System.Text.Json.Serialization;

namespace Domain.Models;
public class UserMembership : BaseEntity
{
    public DateTime StartDate { get; set; }

    public bool IsBlocked { get; set; }

    public string Comment { get; set; }

    public int UserId { get; set; }

    public int MembershipId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Membership Membership { get; set; }

    #endregion
}

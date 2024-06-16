namespace BLL.Infrastructure.Models.Membership;
public class BlockMembershipModel
{
    public bool IsBlocked { get; set; }

    public string Comment { get; set; } = null!;

    public int UserMembershipId { get; set; }
}

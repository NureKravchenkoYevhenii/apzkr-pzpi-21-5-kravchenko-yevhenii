namespace BLL.Infrastructure.Models.UserMembership;
public class UserMembershipModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string StartDate { get; set; } = null!;

    public string EndDate { get; set; } = null!;

    public string? Comment { get; set; }

    public bool IsBlocked { get; set; }
}

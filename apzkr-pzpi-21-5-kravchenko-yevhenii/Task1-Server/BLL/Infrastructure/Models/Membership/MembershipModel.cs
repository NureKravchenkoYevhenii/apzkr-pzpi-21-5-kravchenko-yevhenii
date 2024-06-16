namespace BLL.Infrastructure.Models;
public class MembershipModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }
}

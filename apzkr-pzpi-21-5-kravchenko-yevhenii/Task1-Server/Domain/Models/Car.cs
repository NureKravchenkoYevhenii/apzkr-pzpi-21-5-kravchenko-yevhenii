using System.Text.Json.Serialization;

namespace Domain.Models;
public class Car : BaseEntity
{
    public string CarNumber { get; set; }

	public int UserId { get; set; }

	#region Relations

	[JsonIgnore]
	public User User { get; set; }

	#endregion
}

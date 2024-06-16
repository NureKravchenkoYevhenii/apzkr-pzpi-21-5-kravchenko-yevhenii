using Infrastructure.Enums;

namespace BLL.Infrastructure.Models.Tarif;
public class TarifModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public List<DayOfWeek> ActiveOnDaysOfWeek { get; set; } = new List<DayOfWeek>();

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public TimeUnitValue TimeUnitValue { get; set; }

    public decimal PricePerTimeUnit { get; set; }
}

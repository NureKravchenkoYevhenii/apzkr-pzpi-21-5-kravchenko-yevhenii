using Infrastructure.Enums;

namespace Domain.Models;
public class Tarif : BaseEntity
{
    public string Name { get; set; }

    public string ActiveOnDaysOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public TimeUnitValue TimeUnitValue { get; set; }

    public decimal PricePerTimeUnit { get; set; }
}

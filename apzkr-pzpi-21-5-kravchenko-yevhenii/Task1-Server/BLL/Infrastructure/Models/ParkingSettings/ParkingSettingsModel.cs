using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.ParkingSettings;
public class ParkingSettingsModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    [Range(0, int.MaxValue, ErrorMessage = "RANGE_ERROR_FROM")]
    public int BookingTimeAdvanceInMinutes { get; set; }

    [Required(ErrorMessage = "FIELD_IS_REQUIRED")]
    [Range(0, int.MaxValue, ErrorMessage = "RANGE_ERROR_FROM")]
    public int BookingDurationInMinutes { get; set; }
}

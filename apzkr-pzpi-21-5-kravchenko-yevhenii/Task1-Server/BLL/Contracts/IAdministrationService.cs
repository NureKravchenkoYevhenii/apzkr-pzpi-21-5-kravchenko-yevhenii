using BLL.Infrastructure.Models.ParkingSettings;

namespace BLL.Contracts;
public interface IAdministrationService
{
    void BackupDatabase(string savePath);

    void UpdateParkingSettings(ParkingSettingsModel parkingSettingsModel);

    ParkingSettingsModel GetParkingSettings();
}

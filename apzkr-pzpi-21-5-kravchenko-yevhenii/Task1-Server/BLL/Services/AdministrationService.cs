using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.ParkingSettings;
using DAL.Contracts;
using Domain.Models;
using Infrastructure.Configs;
using Microsoft.Data.SqlClient;

namespace BLL.Services;
public class AdministrationService : IAdministrationService
{
    private readonly Lazy<IUnitOfWork> _unitOfWork;
    private readonly Lazy<IMapper> _mapper;
    private readonly Lazy<ConnectionModel> _connectionModel;

    private readonly Lazy<IRepository<ParkingSettings>> _parkingSettings;

    public AdministrationService(
        Lazy<IUnitOfWork> unitOfWork,
        Lazy<IMapper> mapper,
        Lazy<ConnectionModel> connectionModel)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _connectionModel = connectionModel;

        _parkingSettings = _unitOfWork.Value.GetLazyRepository<ParkingSettings>();
    }

    public void BackupDatabase(string savePath)
    {
        var databaseName = _connectionModel.Value.Database;
        var fullSavePath = string.Format(@"{0}\{1}.bak", savePath, databaseName);
        var query = "BACKUP DATABASE @databaseName TO DISK = @fullSavePath";

        var dbNameParam = new SqlParameter("@databaseName", databaseName);
        var pathParam = new SqlParameter("@fullSavePath", fullSavePath);

        _unitOfWork.Value.ExecuteSqlRaw(query, dbNameParam, pathParam);
    }

    public void UpdateParkingSettings(ParkingSettingsModel parkingSettingsModel)
    {
        var parkingSettings = _parkingSettings.Value
            .Get(x => x.Id == parkingSettingsModel.Id);

        _mapper.Value.Map(parkingSettingsModel, parkingSettings);

        _parkingSettings.Value.Update(parkingSettings);
    }

    public ParkingSettingsModel GetParkingSettings()
    {
        var settings = _parkingSettings.Value
            .GetAll()
            .First();

        var settignsModel = _mapper.Value.Map<ParkingSettingsModel>(settings);

        return settignsModel;
    }
}

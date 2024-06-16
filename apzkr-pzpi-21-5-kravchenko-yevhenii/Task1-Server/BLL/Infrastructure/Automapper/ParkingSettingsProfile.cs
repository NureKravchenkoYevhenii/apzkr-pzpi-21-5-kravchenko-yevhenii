using AutoMapper;
using BLL.Infrastructure.Models.ParkingSettings;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class ParkingSettingsProfile : Profile
{
    public ParkingSettingsProfile()
    {
        CreateMap<ParkingSettings, ParkingSettingsModel>()
            .ReverseMap();
    }
}

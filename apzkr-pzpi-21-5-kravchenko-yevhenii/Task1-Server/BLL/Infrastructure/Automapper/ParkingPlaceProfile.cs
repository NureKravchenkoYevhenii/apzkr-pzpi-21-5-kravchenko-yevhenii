using AutoMapper;
using BLL.Infrastructure.Models.ParkingPlace;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class ParkingPlaceProfile : Profile
{
    public ParkingPlaceProfile()
    {
        CreateMap<ParkingPlace, ParkingPlaceModel>()
            .ReverseMap();

        CreateMap<ParkingPlace, ParkingPlaceListModel>()
            .ForMember(dest => dest.IsOccupied,
                opt => opt.MapFrom(src => src.ParkingSessions.Any(ps => !ps.EndDate.HasValue)))
        .ReverseMap();
    }
}

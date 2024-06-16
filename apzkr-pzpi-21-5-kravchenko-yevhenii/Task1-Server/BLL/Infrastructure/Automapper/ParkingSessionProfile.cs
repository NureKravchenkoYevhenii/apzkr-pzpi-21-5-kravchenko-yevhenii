using AutoMapper;
using BLL.Infrastructure.Models.ParkingSession;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class ParkingSessionProfile : Profile
{
    public ParkingSessionProfile()
    {
        CreateMap<ParkingSession, ParkingSessionModel>()
            .ForMember(dest => dest.StartDate,
                opt => opt.MapFrom(src => src.StartDate.ToLocalTime().ToLongTimeString()))
            .ForMember(dest => dest.EndDate,
                opt => opt.MapFrom(src => src.EndDate == null ? null : src.EndDate.Value.ToLocalTime().ToLongTimeString()))
            .ForMember(dest => dest.ParkingPlaceName,
                opt => opt.MapFrom(src => src.ParkingPlace.Name));
    }
}

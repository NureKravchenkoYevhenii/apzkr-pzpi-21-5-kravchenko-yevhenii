using AutoMapper;
using BLL.Infrastructure.Models.Booking;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<CreateBookingModel, Booking>();
        CreateMap<Booking, BookingModel>()
            .ForMember(dest => dest.BookDate,
                opt => opt.MapFrom(src =>  src.BookDate
                    .ToLocalTime()
                    .ToLongTimeString()))
            .ForMember(dest => dest.ExpireDate,
                opt => opt.MapFrom(src => src.BookDate
                    .Add(src.ExpiresIn)
                    .ToLocalTime()
                    .ToLongTimeString()))
            .ForMember(dest => dest.ParkingPlaceName,
                opt => opt.MapFrom(src => src.ParkingPlace.Name));
    }
}

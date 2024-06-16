using AutoMapper;
using BLL.Infrastructure.Models.Car;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class CarProfile : Profile
{
    public CarProfile()
    {
        CreateMap<Car, CarModel>()
            .ReverseMap();
    }
}

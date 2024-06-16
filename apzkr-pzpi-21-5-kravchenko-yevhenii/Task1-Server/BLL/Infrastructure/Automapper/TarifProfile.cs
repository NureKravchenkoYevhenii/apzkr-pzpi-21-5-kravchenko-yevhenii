using AutoMapper;
using BLL.Infrastructure.Models.Tarif;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class TarifProfile: Profile
{
    public TarifProfile()
    {
        CreateMap<Tarif, TarifModel>()
            .ForMember(dest => dest.ActiveOnDaysOfWeek,
                opt => opt.MapFrom(src => src.ActiveOnDaysOfWeek
                    .Split(",", StringSplitOptions.None)
                    .Select(x => (DayOfWeek)int.Parse(x)).ToList()))
            .ReverseMap()
            .ForMember(dest => dest.ActiveOnDaysOfWeek,
                opt => opt.MapFrom(src => string.Join(',', src.ActiveOnDaysOfWeek.Select(x => (int)x))));
    }
}

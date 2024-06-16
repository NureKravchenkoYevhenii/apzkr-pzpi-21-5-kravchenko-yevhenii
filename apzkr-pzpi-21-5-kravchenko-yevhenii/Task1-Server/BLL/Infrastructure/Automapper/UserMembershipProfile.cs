using AutoMapper;
using BLL.Infrastructure.Models.UserMembership;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class UserMembershipProfile : Profile
{
    public UserMembershipProfile()
    {
        CreateMap<UserMembership, UserMembershipModel>()
            .ForMember(dst => dst.Name,
                opt => opt.MapFrom(src => src.Membership.Name))
            .ForMember(dst => dst.StartDate,
                opt => opt.MapFrom(src => src.StartDate.ToLocalTime().ToLongTimeString()))
            .ForMember(dst => dst.EndDate,
                opt => opt.MapFrom(src => src.StartDate
                    .AddDays(src.Membership.DurationInDays)
                    .ToLocalTime()
                    .ToLongTimeString()));
    }
}

using AutoMapper;
using BLL.Infrastructure.Models;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class MembershipProfile : Profile
{
    public MembershipProfile()
    {
        CreateMap<Membership, MembershipModel>()
            .ReverseMap();
    }
}

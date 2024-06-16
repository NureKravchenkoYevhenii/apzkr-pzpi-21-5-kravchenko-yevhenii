using AutoMapper;
using BLL.Infrastructure.Models.Payment;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentModel>()
            .ForMember(dest => dest.PayDateStr,
                opt => opt.MapFrom(src => src.PayDate.HasValue ? src.PayDate.Value.ToLocalTime().ToLongTimeString() : string.Empty))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.User == null
                    ? null
                    : (src.User.UserProfile.LastName 
                        + " " 
                        + src.User.UserProfile.FirstName).Trim()))
            .ForMember(dest => dest.PayDate,
                opt => opt.MapFrom(src => src.PayDate.HasValue ? (DateTime?)src.PayDate.Value.ToLocalTime() : null))
            .ReverseMap();
    }
}

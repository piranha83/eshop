using AutoMapper;
using Payment.Api.DatabaseContext.Models;
using Payment.Api.Features;

namespace Catalog.Api.Features;

///<inheritdoc/>
public class MapperProfile : Profile
{
    ///<inheritdoc/>
    public MapperProfile()
    {
        CreateMap<PaymentModel, QrCodeResponse>()
            .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(src => src.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(src => src.Payload, opt => opt.MapFrom(src => src.QrCodePayload))
            .ForMember(src => src.Status, opt => opt.MapFrom(src => src.Status));
    }
}
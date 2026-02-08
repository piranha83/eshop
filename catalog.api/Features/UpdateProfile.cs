using AutoMapper;

namespace Catalog.Api.Features;

///<inheritdoc/>
public class UpdateProfile : Profile
{
    public UpdateProfile()
    {
        CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<decimal?, decimal>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        CreateMap<long?, long>().ConvertUsing((src, dest) => src ?? dest);
    }
}
using AutoMapper;
using Catalog.Api.DatabaseContext.Models;

namespace Catalog.Api.Features.Product;

///<inheritdoc/>
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductCreateRequest, ProductModel>()
            .ForMember(src => src.Id, opt => opt.Ignore())
            .ForMember(src => src.Catalog, opt => opt.Ignore())
            .ForMember(src => src.Tags, opt => opt.Ignore());

        CreateMap<ProductUpdateRequest, ProductModel>()
            .ForMember(src => src.Id, opt => opt.Ignore())
            .ForMember(src => src.Catalog, opt => opt.Ignore())
            .ForMember(src => src.Tags, opt => opt.Ignore());

        CreateMap<ProductModel, ProductResponse>();
    }
}
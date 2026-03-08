using AutoMapper;
using Catalog.Api.DatabaseContext.Models;

namespace Catalog.Api.Features.Product;

///<inheritdoc/>
public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagModel, TagResponse>();
    }
}
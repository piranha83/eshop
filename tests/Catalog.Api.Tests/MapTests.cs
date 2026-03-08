using AutoMapper;
using Catalog.Api.Features;
using Catalog.Api.Features.Product;

namespace Catalog.Api.Tests;

public class MapTests
{
    public static MapperConfiguration Default() => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<UpdateProfile>();
        cfg.AddProfile<ProductProfile>();
        cfg.AddProfile<TagProfile>();
    });

    [Fact]
    public void Should_Have_Valid_Configuration()
    {
        // Arrange
        var config = Default();

        // Act & Assert
        config.AssertConfigurationIsValid();
    }
}

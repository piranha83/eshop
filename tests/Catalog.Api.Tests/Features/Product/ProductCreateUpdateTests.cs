using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Features;
using Catalog.Api.Features.Product;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.OutputCaching;
using Moq;

namespace Catalog.Api.Tests;

public class ProductCreateUpdateTests : IAsyncLifetime
{
    private ApplicationDbContext? _context = null;

    public async Task InitializeAsync()
    {
        _context = ApplicationDbContextFactory.Default();
        _context.Database.EnsureCreated();
        _context.Catalogs.Add(new CatalogModel { Id = 1, Name = "Test" });
        _context.Products.Add(new ProductModel { Id = 1, Name = "Test", Description = "1", Price = 11, Rate = 1, CatalogId = 1 });
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context!.DisposeAsync();
    }

    [Fact]
    public async Task Should_Create_Test()
    {
        // Arrange
        var mapper = MapTests.Default().CreateMapper();
        var validator = new ProductCreateRequestValidator();
        var request = new ProductCreateRequest
        {
            CatalogId = 1,
            Name = "Test",
            Description = "1",
            Price = 11,
            Rate = 1,
        };

        var service = new EntityUpdateService<ApplicationDbContext, ProductModel, long, ProductCreateRequest>(
            _context!, mapper, validator);

        // Act
        var id = await service.Add(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, id);
        var product = _context!.Products.FirstOrDefault(x => x.Id == id);
        Assert.NotNull(product);
        Assert.Equal(id, product.Id);
        Assert.Equal(1, product.CatalogId);
        Assert.Equal("Test", product.Name);
        Assert.Equal("1", product.Description);
        Assert.Equal(11, product.Price);
        Assert.Equal(1, product.Rate);
    }


    [Fact]
    public async Task Should_Update_Test()
    {
        // Arrange
        var mapper = MapTests.Default().CreateMapper();
        var validator = new ProductUpdateRequestValidator();
        var request = new ProductUpdateRequest
        {
            CatalogId = 1,
            Name = "Test2",
            Description = "2",
            Price = 22,
            Rate = 2, 
            Discount = 2, 
            InStock = true,
            Img = "img.png"
        };

        var service = new EntityUpdateService<ApplicationDbContext, ProductModel, long, ProductUpdateRequest>(
            _context!, mapper, validator);

        // Act
        var success = await service.Update(1, request, CancellationToken.None);

        // Assert
        Assert.True(success);
        var product = _context!.Products.FirstOrDefault(x => x.Id == 1);
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal(1, product.CatalogId);
        Assert.Equal("Test2", product.Name);
        Assert.Equal("2", product.Description);
        Assert.Equal(22, product.Price);
        Assert.Equal(2, product.Rate);
        Assert.Equal(2, product.Discount);
        Assert.True(product.InStock);
        Assert.Equal("img.png", product.Img);
    }
}

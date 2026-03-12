using Catalog.Api.DatabaseContext;
using Catalog.Api.DatabaseContext.Models;
using Catalog.Api.Features;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.OutputCaching;
using Moq;

namespace Catalog.Api.Tests;

public class ProductDeleteTests : IAsyncLifetime
{
    private ApplicationDbContext? _context = null;

    public async Task InitializeAsync()
    {
        _context = ApplicationDbContextFactory.Default();
        _context.Catalogs.Add(new CatalogModel { Id = 1, Name = "Test" });
        _context.Products.Add(new ProductModel { Id = 1, Name = "Test", Description = "1", Price = 11, Rate = 1, CatalogId = 1 });
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await _context!.DisposeAsync();
    }

    [Fact]
    public async Task Should_Delete_Test()
    {
        // Arrange
        var chache = new Mock<IOutputCacheStore>();
        var service = new EntityDeleteService<ApplicationDbContext, ProductModel, long>(
            _context!, chache.Object);

        // Act
        var success = await service.Delete(1, CancellationToken.None);

        // Assert
        Assert.True(success);
        Assert.False(_context!.Products.Any());
    }
}

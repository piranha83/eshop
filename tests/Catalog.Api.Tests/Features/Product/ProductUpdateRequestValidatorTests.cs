using System.Linq.Expressions;
using Catalog.Api.Features.Product;
using FluentValidation.TestHelper;

namespace Catalog.Api.Tests;

public class ProductUpdateRequestValidatorTests
{
    [Theory]
    [InlineData("Name", true)]
    [InlineData("", false)]
    public void Should_Name_Errors_Test(string value, bool valid)
    {
        // Arrange
        Test(new ProductUpdateRequest
        {
            Name = value!,
            Description = "Description",
            Discount = 1,
            Price = 1,
            Rate = 1,
            CatalogId = 1
        }, valid, x => x.Name);
    }

    [Theory]
    [InlineData("Description", true)]
    [InlineData("", false)]
    public void Should_Description_Errors_Test(string value, bool valid)
    {
        // Arrange
        Test(new ProductUpdateRequest
        {
            Name = "Name",
            Description = value,
            Discount = 1,
            Price = 1,
            Rate = 1,
            CatalogId = 1
        }, valid, x => x.Description);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(-1, false)]
    public void Should_Discount_Errors_Test(int value, bool valid)
    {
        Test(new ProductUpdateRequest
        {
            Name = "Name",
            Description = "Description",
            Discount = value,
            Price = 1,
            Rate = 1,
            CatalogId = 1
        }, valid, x => x.Discount);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(0, false)]
    public void Should_Price_Errors_Test(int value, bool valid)
    {
        Test(new ProductUpdateRequest
        {
            Name = "Name",
            Description = "Description",
            Discount = 1,
            Price = value,
            Rate = 1,
            CatalogId = 1
        }, valid, x => x.Price);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(-1, false)]
    [InlineData(6, false)]
    public void Should_Rate_Errors_Test(int value, bool valid)
    {
        Test(new ProductUpdateRequest
        {
            Name = "Name",
            Description = "Description",
            Discount = 1,
            Price = 1,
            Rate = value,
            CatalogId = 1
        }, valid, x => x.Rate);
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(-1, false)]
    public void Should_Catalog_Errors_Test(int value, bool valid)
    {
        Test(new ProductUpdateRequest
        {
            Name = "Name",
            Description = "Description",
            Discount = 1,
            Price = 1,
            Rate = 1,
            CatalogId = value
        }, valid, x => x.CatalogId);
    }

    private void Test<T>(ProductUpdateRequest model, bool valid, Expression<Func<ProductUpdateRequest, T>> member)
    {
        // Arrange
        var validator = new ProductUpdateRequestValidator();
        // Act
        var result = validator.TestValidate(model);
        // Assets
        Assert.Equal(result.IsValid, valid);
        if (!valid)
        {
            result.ShouldHaveValidationErrorFor(member);
        }
    }
}

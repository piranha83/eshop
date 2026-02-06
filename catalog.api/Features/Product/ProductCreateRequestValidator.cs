using FluentValidation;

namespace Catalog.Api.Features.Product;

///<inheritdoc/>
public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().When(x => x.Description != null);
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        RuleFor(x => x.CatalogId).GreaterThanOrEqualTo(0);
    }
}

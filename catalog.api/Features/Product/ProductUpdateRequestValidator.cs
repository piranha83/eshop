using FluentValidation;

namespace Catalog.Api.Features.Product;

///<inheritdoc/>
public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
{
    public ProductUpdateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().When(x => x.Name != null);
        RuleFor(x => x.Description).NotEmpty().When(x => x.Description != null);
        RuleFor(x => x.Discount).GreaterThanOrEqualTo(0).When(x => x.Discount != null);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(1).When(x => x.Price != null);
        RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5).When(x => x.Rate != null);
        RuleFor(x => x.CatalogId).GreaterThanOrEqualTo(0).When(x => x.CatalogId != null);
    }
}

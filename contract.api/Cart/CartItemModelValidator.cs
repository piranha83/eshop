using FluentValidation;

namespace Contract.Api.Cart;

///<inheritdoc/>
public class CartItemModelValidator : AbstractValidator<CartItemModel>
{
    ///<inheritdoc/>
    public CartItemModelValidator()
    {
        RuleFor(x => x.Product).NotNull().NotEmpty();
        RuleFor(x => x.Count).GreaterThan(0);
    }
}
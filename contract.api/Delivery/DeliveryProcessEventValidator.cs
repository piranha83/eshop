using Contract.Api.Cart;
using FluentValidation;

namespace Contract.Api.Delivery;

///<inheritdoc/>
public class DeliveryProcessEventValidator : AbstractValidator<DeliveryProcessEvent>
{
    ///<inheritdoc/>
    public DeliveryProcessEventValidator(IValidator<CartItemModel> validator)
    {
        RuleFor(x => x.Address).NotNull().NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.CartItems).NotNull().NotEmpty().ForEach(x => x.SetValidator(validator));
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
    }
}
using FluentValidation;

namespace Order.Api.Features.Order;

///<inheritdoc/>
public class OrderModelValidator : AbstractValidator<OrderModel>
{
    ///<inheritdoc/>
    public OrderModelValidator()
    {
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Address).NotNull().NotEmpty();
        RuleFor(x => x.CartItems).NotNull().NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).Must(amount => (amount % 1.00m) <= 0.99m);
    }
}
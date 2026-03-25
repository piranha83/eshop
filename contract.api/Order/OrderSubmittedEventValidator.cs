using FluentValidation;

namespace Contract.Api.Order;

///<inheritdoc/>
public class OrderSubmittedEventValidator : AbstractValidator<OrderSubmittedEvent>
{
    ///<inheritdoc/>
    public OrderSubmittedEventValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Address).NotNull().NotEmpty();
        RuleFor(x => x.CartItems).NotNull().NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0).Must(amount => (amount % 1.00m) <= 0.99m);
    }
}
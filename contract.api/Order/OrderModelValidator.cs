using FluentValidation;

namespace Contract.Api.Order;

///<inheritdoc/>
public class OrderModelValidator : AbstractValidator<OrderModel>
{
    ///<inheritdoc/>
    public OrderModelValidator(IValidator<OrderSubmittedEvent> validator)
    {
        RuleFor(x => x).SetValidator(validator);
    }
}
using FluentValidation;

namespace Contract.Api.Delivery;

///<inheritdoc/>
public class DeliveryProcessEventValidator : AbstractValidator<DeliveryProcessEvent>
{
    ///<inheritdoc/>
    public DeliveryProcessEventValidator()
    {
        RuleFor(x => x.Address).NotNull().NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Type).IsInEnum();
    }
}
using Contract.Api.Payment;
using FluentValidation;

namespace Contract.Api.Delivery;

///<inheritdoc/>
public class PaymentProcessEventValidator : AbstractValidator<PaymentProcessEvent>
{
    ///<inheritdoc/>
    public PaymentProcessEventValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum().Equals(PaymentType.SBP);
        RuleFor(x => x.Currency).IsInEnum();
    }
}
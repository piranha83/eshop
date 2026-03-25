using FluentValidation;

namespace Contract.Api.Payment;

///<inheritdoc/>
public class PaymentProcessEventValidator : AbstractValidator<PaymentProcessEvent>
{
    ///<inheritdoc/>
    public PaymentProcessEventValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
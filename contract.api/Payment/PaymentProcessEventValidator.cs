using FluentValidation;

namespace Contract.Api.Payment;

///<inheritdoc/>
public class PaymentProcessEventValidator : AbstractValidator<PaymentProcessEvent>
{
    ///<inheritdoc/>
    public PaymentProcessEventValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.CurrencyType).IsInEnum();
        RuleFor(x => x.PaymentType).IsInEnum();
        RuleFor(x => x.Amount).GreaterThan(0).Must(amount => (amount % 1.00m) <= 0.99m && (amount % 1.00m) >= 0.01m);
    }
}
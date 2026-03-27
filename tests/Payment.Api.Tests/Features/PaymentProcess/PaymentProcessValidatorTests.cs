using System.Linq.Expressions;
using Contract.Api.Payment;
using FluentValidation.TestHelper;

namespace Payment.Api.Tests.Featres.PaymentProcess;

public class PaymentProcessValidatorTests
{
    [Theory]
    [InlineData("c593aeeb-715c-4845-95e5-249d37b36149", true)]
    [InlineData("00000000-0000-0000-0000-000000000000", false)]
    public void Should_OrderId_Test(string value, bool valid)
    {
        // Arrange
        Test(new Moq
        {
            OrderId = Guid.Parse(value),
            ClientId = Guid.NewGuid(),
            Amount = 0.99m,
            CurrencyType = CurrencyType.Rub,
            PaymentType = PaymentType.SBP,
        }, valid, x => x.OrderId);
    }

    [Theory]
    [InlineData("c593aeeb-715c-4845-95e5-249d37b36149", true)]
    [InlineData("00000000-0000-0000-0000-000000000000", false)]
    public void Should_ClientId_Test(string value, bool valid)
    {
        // Arrange
        Test(new Moq
        {
            OrderId = Guid.NewGuid(),
            ClientId = Guid.Parse(value),
            Amount = 0.99m,
            CurrencyType = CurrencyType.Rub,
            PaymentType = PaymentType.SBP,
        }, valid, x => x.ClientId);
    }

    [Theory]
    [InlineData(0.01, true)]
    [InlineData(0.99, true)]
    [InlineData(0.001, false)]
    [InlineData(0, false)]
    public void Should_Amount_Test(decimal value, bool valid)
    {
        // Arrange
        Test(new Moq
        {
            OrderId = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            Amount = value,
            CurrencyType = CurrencyType.Rub,
            PaymentType = PaymentType.SBP,
        }, valid, x => x.Amount);
    }

    private class Moq : PaymentProcessEvent
    {
        public Guid OrderId { get; init; }

        public Guid ClientId { get; init; }

        public decimal Amount { get; init; }

        public CurrencyType CurrencyType { get; init; }

        public PaymentType PaymentType { get; init; }
    }

    private void Test<T>(PaymentProcessEvent model, bool valid, Expression<Func<PaymentProcessEvent, T>> member)
    {
        // Arrange
        var validator = new PaymentProcessEventValidator();
        // Act
        var result = validator.TestValidate(model);
        // Assets
        Assert.Equal(result.IsValid, valid);
        if (!valid)
        {
            result.ShouldHaveValidationErrorFor(member);
        }
    }
}

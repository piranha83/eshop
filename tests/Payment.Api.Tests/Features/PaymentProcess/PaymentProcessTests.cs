using Contract.Api.Payment;
using Payment.Api.Features.PaymentProcess;
using MassTransit;
using MassTransit.Testing;
using Payment.Api.Features;
using FluentValidation;
using Payment.Api.Features.PaymentStatus;
using NSubstitute;
using Payment.Api.DatabaseContext;

namespace Payment.Api.Tests.Featres.PaymentProcess;

public class PaymentProcess : IAsyncLifetime
{
    ServiceProvider? provider;
    readonly static Guid OrderId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149");

    public async Task DisposeAsync()
    {
        provider?.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        provider = new ServiceCollection()
            .AddTransient(_ =>
            {
                var mock = Substitute.For<IBankSbpService>();
                mock.GenerateQrCode(Arg.Any<QrCodeRequest>(), Arg.Any<CancellationToken>()).Returns(new QrCodeResponse
                {
                    Id = "1",
                    Payload = "https://bank.ru/qr=1",
                    Status = PaymentStatusType.InProgress,
                });
                return mock;
            })
            .AddSingleton(_ => ApplicationDbContextFactory.Default())
            .AddTransient(_ => Substitute.For<ILogger<PaymentProcessEventConsumer>>())
            .AddTransient<IValidator<PaymentProcessEvent>>(_ => new PaymentProcessEventValidator())
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<PaymentProcessEventConsumer>())
            .BuildServiceProvider(true);
    }

    [Fact]
    public async Task PaymentProcessEventConsumer_Test()
    {
        // Arrange
        var harness = provider!.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness!.Bus.Publish<PaymentProcessEvent>(new
        {
            OrderId,
            ClientId = InVar.Id,
            Amount = 0.99m,
            CurrencyType = CurrencyType.Rub,
            PaymentType = PaymentType.SBP,
        });

        var consumerHarness = harness.GetConsumerHarness<PaymentProcessEventConsumer>();

        // Verify the message was consumed by the specific consumer
        Assert.True(await consumerHarness.Consumed.Any<PaymentProcessEvent>());
        // Verify the message was consumed by any endpoint
        Assert.True(await harness.Consumed.Any<PaymentProcessEvent>());

        var consumerIdempotentHarness = harness.GetConsumerHarness<PaymentProcessEventConsumer>();

        // Verify the message was consumed by the specific consumer
        Assert.True(await consumerIdempotentHarness.Consumed.Any<PaymentProcessEvent>());
        // Verify the message was consumed by any endpoint
        Assert.True(await consumerIdempotentHarness.Consumed.Any<PaymentProcessEvent>());

        await harness.Stop();

        // Assert
        var context = provider!.GetRequiredService<ApplicationDbContext>();
        var payment = context.Payments.SingleOrDefault(x => x.OrderId == OrderId);

        Assert.NotNull(payment);
        Assert.Equal(OrderId, payment.OrderId);
        Assert.Equal(0.99m, payment.Amount);
        Assert.Equal(CurrencyType.Rub, payment.Currency);
        Assert.Equal(PaymentType.SBP, payment.Type);
        Assert.Equal("1", payment.QrCodeId);
        Assert.Equal("https://bank.ru/qr=1", payment.QrCodePayload);
        Assert.Equal(PaymentStatusType.InProgress, payment.Status);
    }
}
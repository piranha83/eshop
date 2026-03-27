using Contract.Api.Payment;
using Payment.Api.Features.PaymentStatus;
using MassTransit;
using MassTransit.Testing;
using Payment.Api.Features;
using NSubstitute;
using Payment.Api.DatabaseContext;
using Payment.Api.DatabaseContext.Models;

namespace Payment.Api.Tests.Featres.PaymentStatus;

public class PaymentStatus : IAsyncLifetime
{
    ServiceProvider? provider;

    public async Task DisposeAsync()
    {
        provider?.DisposeAsync();
        await Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        provider = new ServiceCollection()
            .AddTransient(_ =>
            {
                var mock = Substitute.For<IBankSbpService>();
                mock.GetPaymentStatus(Arg.Is("1"), Arg.Any<CancellationToken>())
                    .Returns(PaymentStatusType.NotStarted);
                mock.GetPaymentStatus(Arg.Is("2"), Arg.Any<CancellationToken>())
                    .Returns(PaymentStatusType.Accepted);
                mock.GetPaymentStatus(Arg.Is("3"), Arg.Any<CancellationToken>())
                    .Returns(PaymentStatusType.InProgress);
                mock.GetPaymentStatus(Arg.Is("4"), Arg.Any<CancellationToken>())
                    .Returns(PaymentStatusType.Rejected);
                return mock;
            })
            .AddSingleton(_ => ApplicationDbContextFactory.Default())
            .AddTransient(_ => Substitute.For<ILogger<PaymentStatusEventConsumer>>())
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<PaymentStatusEventConsumer>())
            .BuildServiceProvider(true);
        var context = provider!.GetRequiredService<ApplicationDbContext>();
        context.Payments.AddRange([
            new PaymentModel
            {
                OrderId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149"),
                Amount = 1,
                ClientId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149"),
                Status = PaymentStatusType.InProgress,
                Currency = CurrencyType.Rub,
                Type = PaymentType.SBP,
                QrCodeId = "1",
            },
            new PaymentModel
            {
                OrderId = Guid.Parse("bf0bdabe-73e2-4ec3-8e58-bb1e6384c826"),
                Amount = 1,
                ClientId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149"),
                Status = PaymentStatusType.InProgress,
                Currency = CurrencyType.Rub,
                Type = PaymentType.SBP,
                QrCodeId = "2",
            },
            new PaymentModel
            {
                OrderId = Guid.Parse("485528a8-dd67-4b9e-90cf-19376f1aff05"),
                Amount = 1,
                ClientId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149"),
                Status = PaymentStatusType.InProgress,
                Currency = CurrencyType.Rub,
                Type = PaymentType.SBP,
                QrCodeId = "3",
            },
            new PaymentModel
            {
                OrderId = Guid.Parse("20a83efc-ec14-48f5-ae16-30e50df0db85"),
                Amount = 1,
                ClientId = Guid.Parse("c593aeeb-715c-4845-95e5-249d37b36149"),
                Status = PaymentStatusType.InProgress,
                Currency = CurrencyType.Rub,
                Type = PaymentType.SBP,
                QrCodeId = "4",
            }]);
        await context.SaveChangesAsync();
    }

    [Theory]
    [InlineData("c593aeeb-715c-4845-95e5-249d37b36149")]
    [InlineData("485528a8-dd67-4b9e-90cf-19376f1aff05")]
    [InlineData("20a83efc-ec14-48f5-ae16-30e50df0db85")]
    public async Task PaymentStatusEventConsumer_Rejected_Test(string order)
    {
        // Arrange
        var orderId = Guid.Parse(order);
        var harness = provider!.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness!.Bus.Publish<PaymentStatusEvent>(new
        {
            OrderId = orderId,
        });

        var consumerHarness = harness.GetConsumerHarness<PaymentStatusEventConsumer>();

        // Verify the message was consumed by the specific consumer
        Assert.True(await consumerHarness.Consumed.Any<PaymentStatusEvent>());
        // Verify the message was consumed by any endpoint
        Assert.True(await harness.Consumed.Any<PaymentStatusEvent>());
        // Verify the message was published by any endpoint
        Assert.True(await harness.Published.Any<PaymentCancelledEvent>());

        await harness.Stop();

        // Assert
        var context = provider!.GetRequiredService<ApplicationDbContext>();
        var payment = context.Payments.SingleOrDefault(x => x.OrderId == orderId);

        Assert.NotNull(payment);
        Assert.Equal(PaymentStatusType.Rejected, payment.Status);
    }

    [Theory]
    [InlineData("bf0bdabe-73e2-4ec3-8e58-bb1e6384c826")]
    public async Task PaymentStatusEventConsumer_Accepted_Test(string order)
    {
        // Arrange
        var orderId = Guid.Parse(order);
        var harness = provider!.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness!.Bus.Publish<PaymentStatusEvent>(new
        {
            OrderId = orderId,
        });

        var consumerHarness = harness.GetConsumerHarness<PaymentStatusEventConsumer>();

        // Verify the message was consumed by the specific consumer
        Assert.True(await consumerHarness.Consumed.Any<PaymentStatusEvent>());
        // Verify the message was consumed by any endpoint
        Assert.True(await harness.Consumed.Any<PaymentStatusEvent>());
        // Verify the message was published by any endpoint
        Assert.True(await harness.Published.Any<PaymentReceivedEvent>());

        await harness.Stop();

        // Assert
        var context = provider!.GetRequiredService<ApplicationDbContext>();
        var payment = context.Payments.SingleOrDefault(x => x.OrderId == orderId);

        Assert.NotNull(payment);
        Assert.Equal(PaymentStatusType.Accepted, payment.Status);
    }
}
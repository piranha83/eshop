using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Contract.Api.Cart;
using Contract.Api.Delivery;
using Contract.Api.Product;
using Delivery.Api.Features;
using Delivery.Api.Features.Address;
using Delivery.Api.Features.Delivery;
using Delivery.Api.Features.DeliveryOffer;
using Delivery.Api.Features.DeliveryStatus;
using FluentValidation;
using MassTransit;
using MassTransit.Testing;
using NSubstitute;
using Order.Api.DatabaseContext;

namespace Delivery.Api.Tests.Featres.Delivery;

public class DeliveryServiceTests : IAsyncLifetime
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
                var mock = Substitute.For<IAddressClient>();
                mock.SearchAddressItemAsync(Arg.Any<string>(), Arg.Any<AddressType>(), Arg.Any<CancellationToken>()).Returns(new AddressItem
                {
                    Is_active = true,
                    Object_id = 1
                });
                return mock;
            })
            .AddTransient(_ =>
            {
                var mock = Substitute.For<IDeliveryOfferClient>();
                mock.ConfirmAsync(Arg.Any<Body3>(), Arg.Any<CancellationToken>()).Returns(new Response4
                {
                    Request_id = "1",
                });
                mock.CreateAsync(Arg.Any<Body2>(), Arg.Any<CancellationToken>()).Returns(new Response3
                {
                    Offers =
                    [
                        new Offers2
                        {
                             Offer_id = "1",
                             Station_id = "1",
                             Offer_details = new Offer_details
                             {
                                Pricing = "100 RUB",
                                Pricing_total = "100 RUB",
                                Delivery_interval = new Delivery_interval
                                {
                                    Min = DateTimeOffset.Parse("2025.01.01 00:01:00")
                                }
                             }
                        },
                        new Offers2
                        {
                             Offer_id = "2",
                             Station_id = "2",
                             Offer_details = new Offer_details
                             {
                                Pricing = "200 RUB",
                                Pricing_total = "200 RUB",
                                Delivery_interval = new Delivery_interval
                                {
                                    Min = DateTimeOffset.Parse("2025.01.01 00:01:00")
                                }
                             }
                        },
                        new Offers2
                        {
                             Offer_id = "3",
                             Station_id = "3",
                             Offer_details = new Offer_details
                             {
                                Pricing = "100 RUB",
                                Pricing_total = "100 RUB",
                                Delivery_interval = new Delivery_interval
                                {
                                    Min = DateTimeOffset.Parse("2025.01.01 00:02:00")
                                }
                             }
                        },
                    ]
                });
                return mock;
            })
            .AddTransient<IDeliveryOfferService, DeliveryOfferService>()
            .AddSingleton(_ => ApplicationDbContextFactory.Default())
            .AddTransient(_ => Substitute.For<ILogger<DeliveryProcessEventConsumer>>())
            .AddTransient<IValidator<DeliveryProcessEvent>>(_ => new DeliveryProcessEventValidator(new CartItemModelValidator()))
            .AddMassTransitTestHarness(cfg => cfg.AddConsumer<DeliveryProcessEventConsumer>())
            .BuildServiceProvider(true);
    }

    [Fact]
    public async Task DeliveryProcessEventConsumer_Test()
    {
        // Arrange
        var harness = provider!.GetRequiredService<ITestHarness>();
        await harness.Start();

        // Act
        await harness!.Bus.Publish<DeliveryProcessEvent>(new
        {
            OrderId,
            ClientId = InVar.Id,
            Type = DeliveryType.Athome,
            Address = "Москва Ленинградский проспект 37 к9",
            CartItems = new ReadOnlyCollection<CartItemModel>(
            [
                new CartItemModel
                {
                    Product = new ProductModel
                    {
                        Id = 1,
                        Price = 1.02m,
                    },
                    Count = 1,
                },
            ]),
            FirstName = "Иван",
            Phone = "9012345678",
            Amount = 1.02m,
        });

        var consumerHarness = harness.GetConsumerHarness<DeliveryProcessEventConsumer>();

        // Verify the message was consumed by the specific consumer
        Assert.True(await consumerHarness.Consumed.Any<DeliveryProcessEvent>());
        // Verify the message was consumed by any endpoint
        Assert.True(await harness.Consumed.Any<DeliveryProcessEvent>());

        Assert.True(await harness.Published.Any<DeliveryAcceptedEvent>());

        await harness.Stop();

        // Assert
        var context = provider!.GetRequiredService<ApplicationDbContext>();
        var delivery = context.Deliveries.SingleOrDefault(x => x.OrderId == OrderId);

        Assert.NotNull(delivery);
        Assert.Equal(OrderId, delivery.OrderId);
        Assert.Equal("1", delivery.DeliveryOfferId);
        Assert.Equal(1, delivery.AddressId);
        Assert.Equal(DeliveryStatusType.Accepted, delivery.Status);
    }
}
using System.Collections.ObjectModel;
using Contract.Api.Cart;
using Contract.Api.Product;
using Delivery.Api.Extensions;
using Delivery.Api.Features;
using Delivery.Api.Features.DeliveryOffer;

namespace Delivery.Api.Tests.Featres.DeliveryProcess;

[Trait("Category", "Integration")]
public class DeliveryOfferServiceTests : IAsyncLifetime
{
    ServiceProvider? provider;

    public async Task DisposeAsync()
    {
        provider?.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        var builder = new ConfigurationBuilder();
        var config = builder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AddressApi:Url"] = "https://fias-public-service.nalog.ru/",
            ["AddressApi:Token"] = "bfa2407b-1dc4-4714-9346-b678408eb099",
            ["DeliveryOfferApi:Url"] = "https://b2b.taxi.tst.yandex.net",
            ["DeliveryOfferApi:Token"] = "y2_AgAAAAD04omrAAAPeAAAAAACRpC94Qk6Z5rUTgOcTgYFECJllXYKFx8",
            ["DeliveryOfferApi:SourceStationId"] = "e1139f6d-e34f-47a9-a55f-31f032a861a6",
            ["DeliveryOfferApi:DestanationStationId"] = "01946f4f013c7337874ec2fb848a58a4",
        }).Build();

        provider = new ServiceCollection()
            .AddDeliveryOffer(config)
            .BuildServiceProvider(true);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task CancelConfirmed_Valid_Test()
    {
        // Arrange
        var service = provider!.GetRequiredService<IDeliveryOfferService>();

        // Act
        var delivery = await service.Create(new DeliveryOfferCreateRequest
        {
            OrderId = Guid.NewGuid(),
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
        }, default);

        var canceled = await service.Cancel(delivery.DeliveryOfferId, default);

        // Assert
        Assert.NotNull(delivery);
        Assert.NotEmpty(delivery.DeliveryOfferId);
        Assert.NotEqual(0, delivery.AddressId);
        Assert.True(canceled);
    }
}
using Delivery.Api.Features.Address;
using Delivery.Api.Features.DeliveryStatus;
using Microsoft.Extensions.Options;

namespace Delivery.Api.Features.DeliveryOffer;

///<inheritdoc/>
internal class DeliveryOfferService(
    IDeliveryOfferClient deliveryOfferClient,
    IAddressClient addressClient,
    IOptions<DeliveryOfferOption> options,
    ILogger<DeliveryOfferService> logger)
    : IDeliveryOfferService
{
    ///<inheritdoc/>
    public async Task<bool> Cancel(string deliveryOfferId, CancellationToken cancellationToken)
    {
        var canceledResponse = await deliveryOfferClient.CancelAsync(new Body4
        {
            Request_id = deliveryOfferId,
        }, cancellationToken);

        logger.LogInformation($"Статус {canceledResponse.Status}, причина {canceledResponse.Reason}");

        return canceledResponse.Status != nameof(DeliveryOfferStatusType.ERROR);
    }

    ///<inheritdoc/>
    public async Task<DeliveryOfferCreateResponse> Create(DeliveryOfferCreateRequest request, CancellationToken cancellationToken)
    {
        // Проверить адресс доставки.
        var addressResponce = await addressClient.SearchAddressItemAsync(
            request.Address.Trim(),
            AddressType._2,
            cancellationToken);
        if (!addressResponce.Is_active)
        {
            logger.LogError($"Адресс {request.Address} не корректный.");
            throw new OperationCanceledException("Адресс не корректный.");
        }

        // Создание заявки.
        var barcode = $"BC-{Guid.NewGuid()}";
        var deliveryResponse = await deliveryOfferClient.CreateAsync(new Body2
        {
            Billing_info = new Billing_info
            {
                Payment_method = "already_paid",
            },
            Destination = new Destination2
            {
                Platform_station = new Platform_station2
                {
                    Platform_id = options.Value.DestanationStationId,
                }
            },
            Info = new Info
            {
                Operator_request_id = request.OrderId.ToString(),
                Comment = $"Заказ {request.OrderId} от {DateTimeOffset.UtcNow}",
            },
            Items = request.CartItems.Select(x => new Items
            {
                Article = $"ATC-{x.Product.Id}",
                Billing_details = new Billing_details
                {
                    // в копейках
                    Assessed_unit_price = (int)(x.Product.Price * 100),
                    Unit_price = (int)(x.Product.Price * 100),
                },
                Count = x.Count,
                Name = $"Продукт-{x.Product.Id}",
                Place_barcode = barcode,
            }).ToList(),
            Last_mile_policy = "time_interval",
            Places =
            [
                new Places
                {
                    Barcode = barcode,
                    Physical_dims = new Physical_dims2
                    {
                        Predefined_volume = 1500,
                    }
                }
            ],
            Recipient_info = new Recipient_info
            {
                Phone = request.Phone,
                First_name = request.FirstName,
            },
            Source = new Source2
            {
                Platform_station = new Platform_station
                {
                    Platform_id = options.Value.SourceStationId,
                }
            },
        }, cancellationToken);

        // Лучшее предложение.
        var deliveryOffer = deliveryResponse.Offers
            .OrderBy(x => decimal.Parse(x.Offer_details.Pricing.TrimEnd("RUB ".ToCharArray()))).ThenBy(x => x.Offer_details.Delivery_interval.Min)
            .FirstOrDefault();

        if (deliveryOffer == null)
        {
            logger.LogError("Предложений доставки нет.");
            throw new OperationCanceledException("Предложений доставки нет.");
        }

        // Принять заказ.
        var deliveryOfferResponse = await deliveryOfferClient.ConfirmAsync(new Body3
        {
            Offer_id = deliveryOffer.Offer_id,
        }, cancellationToken);

        if (string.IsNullOrWhiteSpace(deliveryOfferResponse.Request_id))
        {
            logger.LogError("Доставки нет.");
            throw new OperationCanceledException("Доставки нет.");
        }

        return new DeliveryOfferCreateResponse
        {
            DeliveryOfferId = deliveryOfferResponse.Request_id,
            AddressId = addressResponce.Object_id,
        };
    }

    ///<inheritdoc/>
    public async Task<DeliveryOfferResponse> Details(string deliveryOfferId, CancellationToken cancellationToken)
    {
        var deliveryOfferResponse = await deliveryOfferClient.InfoGET2Async(deliveryOfferId, cancellationToken);

        return new DeliveryOfferResponse
        {
            DeliveryOfferId = deliveryOfferResponse.Request_id,
            Price = deliveryOfferResponse.Full_items_price / 100,
            Status = deliveryOfferResponse.State.Status switch
            {
                nameof(DeliveryOfferStatusType.CREATED) => DeliveryStatusType.Accepted,
                _ => DeliveryStatusType.Canceled,
            }
        };
    }
}
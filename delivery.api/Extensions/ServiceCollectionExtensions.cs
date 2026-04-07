using Delivery.Api.Features;
using Delivery.Api.Features.Address;
using Delivery.Api.Features.Delivery;
using Delivery.Api.Features.DeliveryOffer;
using FluentValidation;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Api.DatabaseContext;
using Order.Api.Featres.Job;

namespace Delivery.Api.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBuisness(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.GetValue<string>("Rabbit"));

        serviceCollection.AddHostedService<InitJob>();
        serviceCollection.AddContext<ApplicationDbContext>(configuration);
        serviceCollection.AddDeliveryOffer(configuration);

        serviceCollection.AddCrudServices();

        serviceCollection.AddValidatorsFromAssemblies([typeof(ContractApi).Assembly, typeof(Program).Assembly]);

        serviceCollection.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<DeliveryProcessEventConsumer>();
            cfg.AddConsumer<DeliveryProcessEventFaultConsumer>();

            cfg.AddEntityFrameworkOutbox<ApplicationDbContext>(config =>
            {
                config.UsePostgres();

                config.UseBusOutbox();
                config.DisableInboxCleanupService();
                //config.QueryDelay = TimeSpan.FromMinutes(4);
            });

            cfg.UsingRabbitMq((context, config) =>
            {
                config.PrefetchCount = 1;
                config.Host(configuration.GetValue<string>("Rabbit"));
                config.ConfigureEndpoints(context);
            });
        });

        serviceCollection.AddOptions<MassTransitHostOptions>().Configure(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromSeconds(60);
        });

        serviceCollection.AddOptions<HostOptions>().Configure(options =>
        {
            options.StartupTimeout = TimeSpan.FromSeconds(60);
            options.ShutdownTimeout = TimeSpan.FromSeconds(60);
        });

        return serviceCollection;
    }

    internal static IServiceCollection AddDeliveryOffer(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
    {
        serviceCollection.AddHttpClient<IAddressClient, AddressClient, AddressHandler, AddressOption>(configuration.GetSection("AddressApi"));
        serviceCollection.AddHttpClient<IDeliveryOfferClient, DeliveryOfferClient, DeliveryOfferHandler, DeliveryOfferOption>(configuration.GetSection("DeliveryOfferApi"));
        serviceCollection.AddTransient<IDeliveryOfferService, DeliveryOfferService>();

        return serviceCollection;
    }
}
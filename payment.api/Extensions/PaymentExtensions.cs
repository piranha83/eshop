using Payment.Api.Features.PaymentProcess;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.Api.DatabaseContext;
using Payment.Api.Featres.Job;
using Payment.Api.Features.TochkaBank;
using Payment.Api.Features;
using Microsoft.Extensions.Options;
using Payment.Api.Features.PaymentStatus;
using Payment.Api.Features.PaymentRefund;
using Infrastructure.Core;

namespace Payment.Api.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class PaymentExtensions
{
    public static IServiceCollection AddPaymenProcess(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(configuration.GetValue<string>("Rabbit"));
        serviceCollection.AddHostedService<InitJob>();
        serviceCollection.AddContext<ApplicationDbContext>(configuration);
        serviceCollection.AddTochkaBank(configuration);

        serviceCollection.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<PaymentProcessEventConsumer>(x => x.UseConcurrentMessageLimit(1));
            cfg.AddConsumer<PaymentProcessEventFaultConsumer>(x => x.UseConcurrentMessageLimit(1));
            cfg.AddConsumer<PaymentStatusEventConsumer>(x => x.UseConcurrentMessageLimit(1));
            cfg.AddConsumer<PaymentRefundEventConsumer>(x => x.UseConcurrentMessageLimit(1));

            cfg.AddEntityFrameworkOutbox<ApplicationDbContext>(config =>
            {
                config.UsePostgres();
                config.UseBusOutbox();
            });

            cfg.UsingRabbitMq((context, config) =>
            {
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

    public static IServiceCollection AddTochkaBank(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(configuration);

        serviceCollection.AddHttpClient<ITochkaBankClient, TochkaBankClient, TochkaBankClientHandler, TochkaBankOptions>(configuration.GetSection("TochkaBankApi"));
        serviceCollection.AddTransient<IBankSbpService, TochkaBankService>();
        serviceCollection.AddTransient<TochkaBankClientHandler>();

        return serviceCollection;
    }
}
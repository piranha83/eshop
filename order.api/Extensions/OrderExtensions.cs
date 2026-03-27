using FluentValidation;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Order.Api.DatabaseContext;
using Order.Api.Featres.Job;
using Order.Api.Features.Order;

namespace Order.Api.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class OrderExtensions
{
    public static IServiceCollection AddOrderLogic(
            this IServiceCollection serviceCollection,
            IConfiguration configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(configuration.GetConnectionString("Default"));

        var options = configuration.GetSection("Rabbit").Get<RabbitOptions>();
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Host);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.User);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Password);

        serviceCollection.AddValidatorsFromAssemblies([typeof(ContractApi).Assembly, typeof(Program).Assembly]);

        serviceCollection.AddHostedService<OrderJob>();

        serviceCollection.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("Default")));

        serviceCollection.AddMassTransit(cfg =>
        {
            cfg.AddDelayedMessageScheduler();
            cfg.AddSagaStateMachine<OrderSaga, OrderState>().EntityFrameworkRepository(config =>
            {
                config.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                config.ExistingDbContext<ApplicationDbContext>();
                config.UsePostgres();
            });
            cfg.UsingRabbitMq((context, config) =>
            {
                config.UseDelayedMessageScheduler();
                config.Host(options.Host, options.Vhost, h =>
                {
                    h.Username(options.User);
                    h.Password(options.Password);
                    if (options.UseSsl)
                    {
                        h.UseSsl();
                    }
                });

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

        serviceCollection.AddTransient<IOrderServcie, OrderServcie>();
        serviceCollection.AddDefaultContext();

        return serviceCollection;
    }

    /// <summary>
    /// Создать заказ.
    /// </summary>
    /// <param name="wwebApplicationb">HTTP pipeline.</param>
    public static void AddOrderHandler(this WebApplication webApplication)
    {
        webApplication.MapPost("order", async (
            [FromServices] IOrderServcie orderServcie,
            [FromBody] OrderModel reqest,
            CancellationToken ct) =>
        {
            var orderId = await orderServcie.Create(reqest, ct);
            return Results.Created($"order/{orderId}", orderId);
        })
        .Produces(StatusCodes.Status201Created, typeof(Guid), contentType: "application/json")
        .Produces(StatusCodes.Status500InternalServerError)
        .ProducesValidation()
        .AllowAnonymous()
        //.RequireAuthorization(Consts.Policy.Viewer)
        .WithDescription("Создать заказ.");
    }
}
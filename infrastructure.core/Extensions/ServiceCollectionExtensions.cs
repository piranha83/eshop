using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddContext<TContext>(this IServiceCollection serviceCollection, IConfiguration configuration)
    where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(serviceCollection, nameof(serviceCollection));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        return serviceCollection
            .AddDbContext<TContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));
    }

    public static async Task EnsureCreated<TContext>(this IServiceScope scope)
    where TContext : DbContext
    {
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.EnsureCreatedAsync();
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection, string policy, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(configuration);

        var origin = configuration.GetSection(Consts.ApplicationOrigin).Value;
        ArgumentException.ThrowIfNullOrEmpty(origin);

        return serviceCollection.AddCors(options => options.AddPolicy(policy, policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(origin)));
    }

    public static IServiceCollection AddMapper(this IServiceCollection serviceCollection, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        ArgumentNullException.ThrowIfNull(assembly);

        return serviceCollection.AddAutoMapper(assembly);
    }

    public static IServiceCollection AddCrudServices(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        return serviceCollection
            .AddTransient(typeof(IEntityReadService<,,,>), typeof(EntityReadService<,,,>))
            .AddTransient(typeof(IEntityUpdateService<,,,>), typeof(EntityUpdateService<,,,>))
            .AddTransient(typeof(IEntityDeleteService<,,>), typeof(EntityDeleteService<,,>));
    }
}
using System.Reflection;
using FluentValidation;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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
        try
        {
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            var databaseCreator = context.Database.GetService<IRelationalDatabaseCreator>();
            if (await databaseCreator.CanConnectAsync())
            {
                await databaseCreator.CreateTablesAsync();
            }
        }
        catch (PostgresException ex) when (ex.MessageText.Contains("already exists"))
        {
            //Riden hot fix
        }
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        return serviceCollection.AddCors(options => options.AddPolicy("Policy", policy => policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(configuration.GetCatalogApiUrl(), configuration.GetIdentityApiUrl())));
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

    public static async Task Validate<TEntity>(this IValidator<TEntity> validator, TEntity model)
    {
        var validationResult = await validator.ValidateAsync(model);
        if (!validationResult.IsValid)
            throw new ValidationApiException(validationResult.ToDictionary());
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
    {
        return app.UseCors("Policy");
    }
}
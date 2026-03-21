using System.Reflection;
using FluentValidation;
using Infrastructure.Core.Features.Context;
using Infrastructure.Core.Features.Entity;
using Infrastructure.Core.Features.HealthCheck;
using Infrastructure.Core.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
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
            .AddDbContext<TContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("Default"))
                .AddInterceptors(new SelectForUpdateInterceptor()));
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
            .WithOrigins(configuration.GetCatalogApiUrl(), configuration.GetIdentityApiUrl(), configuration.FrontendUrl())));
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

    public static IHealthChecksBuilder AddHealthCheck(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        return serviceCollection.AddHealthChecks()
            .AddTypeActivatedCheck<NpgSqlHealthCheck>(nameof(NpgSqlHealthCheck), failureStatus: HealthStatus.Degraded);
    }

    public static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder app)
    {
        return app.MapHealthChecks("/health-check", new HealthCheckOptions
        {
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            },
        });
    }

    public static void AddSecrets(this IConfigurationManager configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(configuration["secrets-path"]);
        Console.WriteLine($"Attach {configuration["secrets-path"]!}");

        configuration.AddKeyPerFile(configuration["secrets-path"]!, true, false);
    }

    public static IServiceCollection AddDefaultContext(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        serviceCollection.AddTransient<IContextAccessor, ContextAccessor>();
        serviceCollection.AddHttpContextAccessor();
        return serviceCollection;
    }

    public static IServiceCollection AddCache(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        return serviceCollection.AddOutputCache(options =>
        {
            options.AddPolicy(Consts.Cache.FilterPolicy, builder => builder
                .Tag(Consts.Cache.FilterPolicy)
                .Expire(configuration.GetMinutes("CatalogApi:CacheMinutes", TimeSpan.FromMinutes(5))));
            options.AddPolicy(Consts.Cache.NoCache, builder => builder
                .NoCache());
        });
    }

    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);

        serviceCollection.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            
            option.AddSecurityRequirement(document =>
            {
                var requirement = new OpenApiSecurityRequirement();
                requirement.Add(new OpenApiSecuritySchemeReference("Bearer", document), []);
                return requirement;
            });
        });
    }
}
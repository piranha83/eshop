using Identity.Api.DatabaseContext;
using Identity.Api.Featres.Job;
using Identity.Api.Featres.User;
using Infrastructure.Core;
using Infrastructure.Core.Features.Security;
using Infrastructure.Core.Interceptors;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace Identity.Api.Featres.Flow;

internal static class FlowExtensions
{
    internal static IServiceCollection AddFlowServer(this IServiceCollection services,
        IConfigurationManager configuration,
        ISecurityContext securityContext)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(securityContext);

        // Configure the asp.core auth
        services.AddAuthorization().AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services
            .AddTransient<IUserService, UserService>()
            .AddTransient<IFlowService, PasswordFlowService>()
            .AddTransient<IInitService, InitService>()
            .AddHostedService<IdentityJob>()
            .AddOpenIddict();

        // Configure the OpenIddict to use pgsql. 
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("Default"))
            .AddInterceptors(new SelectForUpdateInterceptor())
            //Register the entity sets needed by OpenIddict.
            .UseOpenIddict())

        // Register the OpenIddict core components.
        .AddOpenIddict().AddCore(options => options
            .UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>())

        // Register the OpenIddict server components.
        .AddServer(options => options
            .AddPasswordFlowServer()
            .AddSigningKeys(securityContext.CreateSigningKeys())
            .AddEncryptionKeys(securityContext.CreateEncryptionKeys())
            .UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableEndSessionEndpointPassthrough());

        return services;
    }

    internal static void UseFlowServer(this WebApplication app)
    {
        app.UseAuthorization().UseAuthentication();
        app.MapMethods(Consts.TokenEndpointUrl, [HttpMethods.Post], (IFlowService service) => service.Token());
        //app.MapMethods(Consts.UnauthorizeUrl, [HttpMethods.Post], (IFlowService service) => service.Token());
    }

    internal static OpenIddictServerBuilder AddSigningCertificate(this OpenIddictServerBuilder serverBuilder)
    {
        using var stream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificates", "server-signing-certificate.pfx"), FileMode.Open);
        serverBuilder.AddSigningCertificate(stream, null);
        return serverBuilder;
    }

    private static OpenIddictServerBuilder AddPasswordFlowServer(this OpenIddictServerBuilder options) =>
        options.SetTokenEndpointUris(Consts.TokenEndpointUrl)
            //.SetEndSessionEndpointUris(Consts.UnauthorizeUrl)
            .AllowPasswordFlow()
            //.AllowRefreshTokenFlow()
            .SetAccessTokenLifetime(TimeSpan.FromMinutes(15))
            .SetRefreshTokenLifetime(TimeSpan.FromHours(2));
}
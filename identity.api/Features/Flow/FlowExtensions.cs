using System.Security.Cryptography;
using System.Text;
using Identity.Api.DatabaseContext;
using Identity.Api.Featres.Job;
using Identity.Api.Featres.User;
using Infrastructure.Core;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

namespace Identity.Api.Featres.Flow;

internal static class FlowExtensions
{
    internal static IServiceCollection AddFlowServer(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

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
            //Register the entity sets needed by OpenIddict.
            .UseOpenIddict())

        // Register the OpenIddict core components.
        .AddOpenIddict().AddCore(options => options
            .UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>())

        // Register the OpenIddict server components.
        .AddServer(options => options
            .AddPasswordFlowServer()
            // Certificate
            .AddSigningCertificate().AddEncryptionCertificate(configuration)
            // Handle authorization requests in a MVC.
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

    internal static OpenIddictServerBuilder AddEncryptionCertificate(this OpenIddictServerBuilder serverBuilder, IConfiguration configuration)
    {
        if (configuration.IsRender()) /*for tests*/
        {
            serverBuilder.AddEphemeralEncryptionKey().DisableAccessTokenEncryption();
        }
        else
        {
            using var stream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "certificates", "server-encryption-certificate.pfx"), FileMode.Open);
            serverBuilder.AddEncryptionCertificate(stream, null);            
        }

        return serverBuilder;
    }

    internal static byte[] Hash512(this string? password) =>
        SHA512.HashData(Encoding.UTF8.GetBytes(password ?? ""));

    private static OpenIddictServerBuilder AddPasswordFlowServer(this OpenIddictServerBuilder options) =>
        options.SetTokenEndpointUris(Consts.TokenEndpointUrl)
            //.SetEndSessionEndpointUris(Consts.UnauthorizeUrl)
            .AllowPasswordFlow()
            //.AllowRefreshTokenFlow()
            .SetAccessTokenLifetime(TimeSpan.FromMinutes(15))
            .SetRefreshTokenLifetime(TimeSpan.FromHours(24));
}
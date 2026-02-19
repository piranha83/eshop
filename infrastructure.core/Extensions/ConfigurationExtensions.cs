using Microsoft.Extensions.Configuration;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// ConfigurationExtensions.
/// </summary>
public static class ConfigurationExtensions
{
    public static string GetCatalogApiUrl(this IConfiguration configuration) =>
        configuration.GetString("CatalogApi:Url");

    public static string GetIdentityApiUrl(this IConfiguration configuration) =>
        configuration.GetString("IdentityApi:Url");

    public static bool IsRender(this IConfiguration configuration) =>
        !string.IsNullOrEmpty(configuration["render"]);

    private static string GetString(this IConfiguration configuration, string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);

        var value = configuration[key];
        ArgumentException.ThrowIfNullOrEmpty(value);

        return value;
    }
}
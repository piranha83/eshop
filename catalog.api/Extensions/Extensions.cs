using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Catalog.Api.Extensions;

internal static class Extensions
{
    internal static void UseImgFiles(this WebApplication application, string contentRootPath)
    {
        ArgumentNullException.ThrowIfNull(contentRootPath, nameof(contentRootPath));

        application.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(contentRootPath, "img")),
            RequestPath = "/img",
            DefaultContentType = "application/jpg",
        });
    }
}
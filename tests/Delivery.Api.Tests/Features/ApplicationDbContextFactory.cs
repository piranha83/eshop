using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Order.Api.DatabaseContext;

namespace Delivery.Api.Tests.Featres;

internal static class ApplicationDbContextFactory
{
    public static DbContextOptions Options(DbContextOptionsBuilder dbContextOptionsBuilder) =>
        dbContextOptionsBuilder
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

    public static ApplicationDbContext Default()
    {
        var context = new ApplicationDbContext(Options(new DbContextOptionsBuilder<ApplicationDbContext>()));
        context.Database.EnsureCreated();
        return context;
    }
}
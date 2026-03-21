namespace Order.Api.DatabaseContext;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MassTransit;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("delivery");
        base.OnModelCreating(modelBuilder);

        // UseBusOutbox mode.
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        // Config
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
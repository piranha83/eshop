namespace Order.Api.DatabaseContext;

using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Delivery.Api.DatabaseContext.Models;
using Infrastructure.Core.Features.Context;
using Infrastructure.Core.Extensions;
using MassTransit;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    public DbSet<DeliveryModel> Deliveries { get; set; }

    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Редактируемый пользователь.
    /// </summary>
    private readonly User? _user = null;

    ///<inheritdoc/>
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        base.ChangeTracker.Tracker(_user?.UserId);
        return await base.SaveChangesAsync(cancellationToken);
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
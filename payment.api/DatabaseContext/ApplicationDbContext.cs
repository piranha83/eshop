namespace Payment.Api.DatabaseContext;

using System.Reflection;

using Infrastructure.Core.Features.Context;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Payment.Api.DatabaseContext.Models;
using MassTransit;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<PaymentModel> Payments { get; set; }

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
        modelBuilder.HasDefaultSchema("payment");
        base.OnModelCreating(modelBuilder);

        // UseBus Inbox/Outbox mode.
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        // Config
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
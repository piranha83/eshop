namespace Identity.Api.DatabaseContext;

using Identity.Api.DatabaseContext.Models;
using Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// Пользователи.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    ///<inheritdoc/>
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        base.ChangeTracker.Tracker(null);
        return await base.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");
        base.OnModelCreating(modelBuilder);
    }
}
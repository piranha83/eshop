namespace Identity.Api.DatabaseContext;

using Identity.Api.DatabaseContext.Models;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Context;
using Microsoft.EntityFrameworkCore;

///<inheritdoc/>
internal class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Пользователи.
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Редактируемый пользователь.
    /// </summary>
    private readonly User? _user = null;

    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    ///<inheritdoc/>
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        base.ChangeTracker.Tracker(_user?.UserId);
        return await base.SaveChangesAsync(cancellationToken);
    }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("identity");
        base.OnModelCreating(modelBuilder);
    }
}
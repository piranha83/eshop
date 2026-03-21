namespace Payment.Api.DatabaseContext;

using System.Reflection;
using Microsoft.EntityFrameworkCore;

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
        modelBuilder.HasDefaultSchema("payment");
        base.OnModelCreating(modelBuilder);

        // Config
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
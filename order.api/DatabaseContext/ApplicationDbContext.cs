using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Order.Api.DatabaseContext;

///<inheritdoc/>
public class ApplicationDbContext : DbContext
{
    ///<inheritdoc/>
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("order");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
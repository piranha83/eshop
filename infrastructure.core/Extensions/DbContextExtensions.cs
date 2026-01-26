using System.Security.Principal;
using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class DbContextExtensions
{
    public static void Tracker(this ChangeTracker changeTracker, IIdentity? identity)
    {
        ArgumentNullException.ThrowIfNull(changeTracker, nameof(changeTracker));
        ArgumentNullException.ThrowIfNull(identity, nameof(identity));

        if (identity?.IsAuthenticated == true && Guid.TryParse(identity?.Name, out Guid editedUserId))
        {
            foreach (var entityEntry in changeTracker.Entries<IUpdated>().Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified))
            {
                entityEntry.Entity.UpdatedDate = DateTimeOffset.UtcNow;
                entityEntry.Entity.Updated = editedUserId;
            }
        }
    }
}
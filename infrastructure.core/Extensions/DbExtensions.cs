using System.Globalization;
using System.Reflection;
using System.Security.Principal;
using Infrastructure.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class DbExtensions
{
    private static BindingFlags Default = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

    public static IQueryable<TEntity> Search<TEntity, TKey>(this IQueryable<TEntity> data, SearchCriteria searchCriteria)
    where TEntity : class, IEntity<TKey>
    where TKey: struct
    {
        if (searchCriteria.Editable != true)
            data = data.AsNoTracking();
        if (searchCriteria.Include != null)
            data = data.Include(searchCriteria.Include.Trim());
        if (searchCriteria.Term != null)
            data = data.Where(searchCriteria.Term);
        if (searchCriteria.SortField != null && searchCriteria.SortDirection != null)
            data = data.OrderBy(searchCriteria.SortField, searchCriteria.SortDirection.Value);
        if (searchCriteria.Skip != null && searchCriteria.Take != null)
            data = data.Skip((searchCriteria.Skip.Value - 1) * searchCriteria.Take.Value);
        if (searchCriteria.Take != null)
            data = data.Take(searchCriteria.Take.Value);
        return data;
    }

    public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, Dictionary<string, StringValues> pairs)
    {
        foreach (var pair in pairs)
        {
            var id = pair.Key.LastIndexOf("_");
            var pn = id != -1 ? pair.Key[..id] : pair.Key;
            var property = typeof(TEntity).GetProperty(pn, Default);
            ArgumentNullException.ThrowIfNull(property);

            var opType = id != -1 ? pair.Key[id..] : null;

            if (property!.PropertyType == typeof(Guid) && Guid.TryParse(pair.Value, out Guid key))
            {
                data = data.Where(opType!, property.Name, key);
            }
            else if (property!.PropertyType == typeof(bool) && bool.TryParse(pair.Value, out bool flag))
            {
                data = data.Where(opType!, property.Name, flag);
            }
            else if (property!.PropertyType == typeof(decimal) && decimal.TryParse(pair.Value, out decimal dec))
            {
                data = data.Where(opType!, property.Name, dec);
            }
            else if (property!.PropertyType == typeof(double) && double.TryParse(pair.Value, out double d))
            {
                data = data.Where(opType!, property.Name, d);
            }
            else if (property!.PropertyType == typeof(int) && int.TryParse(pair.Value, out int i))
            {
                data = data.Where(opType!, property.Name, i);
            }
            else if (property!.PropertyType == typeof(DateTime) &&
                DateTime.TryParseExact(pair.Value, "yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                data = data.Where(opType!, property.Name, dt);
            }
            else if (property!.PropertyType == typeof(DateTimeOffset) &&
                DateTimeOffset.TryParseExact(pair.Value, "yyyy.MM.dd HH:mm:ss zzz", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset dto))
            {
                data = data.Where(opType!, property.Name, dto);
            }
            else
            {
                data = data.Where(opType!, property.Name, pair.Value.ToString());
            }
        }

        return data;
    }

    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> data, string propertyName, Direction direction) =>
    direction == Direction.Desc
        ? data.OrderByDescending(x => EF.Property<dynamic>(x!, propertyName))
        : data.OrderBy(x => EF.Property<dynamic>(x!, propertyName));

    public static void Tracker(this ChangeTracker changeTracker, IIdentity? identity = null)
    {
        ArgumentNullException.ThrowIfNull(changeTracker, nameof(changeTracker));

        foreach (var entityEntry in changeTracker.Entries<IUpdated>().Where(e =>
            e.State == EntityState.Added ||
            e.State == EntityState.Modified))
        {
            entityEntry.Entity.UpdatedDate = DateTimeOffset.UtcNow;
            if (identity?.IsAuthenticated == true && Guid.TryParse(identity?.Name, out Guid editedUserId))
            {
                entityEntry.Entity.Updated = editedUserId;
            }
        }
    }

    private static IQueryable<TEntity> Where<TEntity, TValue>(this IQueryable<TEntity> data, string opType, string propertyName, TValue value) =>
    opType switch
    {
        "_ne" => data.Where(x => !EF.Property<TValue>(x!, propertyName)!.Equals(value)),
        "_lt" => data.Where(x => EF.Functions.LessThan(ValueTuple.Create(EF.Property<TValue>(x!, propertyName)), ValueTuple.Create(value))),
        "_gt" => data.Where(x => EF.Functions.GreaterThan(ValueTuple.Create(EF.Property<TValue>(x!, propertyName)), ValueTuple.Create(value))),
        "_lte" => data.Where(x => EF.Functions.LessThanOrEqual(ValueTuple.Create(EF.Property<TValue>(x!, propertyName)), ValueTuple.Create(value))),
        "_gte" => data.Where(x => EF.Functions.GreaterThanOrEqual(ValueTuple.Create(EF.Property<TValue>(x!, propertyName)), ValueTuple.Create(value))),
        _ => data.Where(x => EF.Property<TValue>(x!, propertyName)!.Equals(value)),
    };
}
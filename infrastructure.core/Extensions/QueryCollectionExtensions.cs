using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class QueryCollectionExtensions
{
    private static BindingFlags Default = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

    public static Dictionary<string, StringValues> ToTerm(this IQueryCollection query)
    {
        return query.Where(x =>
                x.Key != "_page" &&
                x.Key != "_limit" &&
                x.Key != "_sort" &&
                x.Key != "_order" &&
                x.Key != "_include")
                .ToDictionary(x => x.Key, x => x.Value);
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

            if (property!.PropertyType == typeof(bool) && bool.TryParse(pair.Value, out bool flag))
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
            else
            {
                data = data.Where(opType!, property.Name, pair.Value.ToString());
            }
        }

        return data;
    }

    public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> data, string propertyName, Direction direction)
    {
        var property = typeof(TEntity).GetProperty(propertyName, Default);
        ArgumentNullException.ThrowIfNull(property);

        return direction == Direction.Desc
            ? data.OrderByDescending(x => EF.Property<dynamic>(x!, property.Name))
            : data.OrderBy(x => EF.Property<dynamic>(x!, property.Name));   
    }

    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, string opType, string propertyName, int value)
    => opType switch
    {
        "_ne" => data.Where(x => EF.Property<int>(x!, propertyName) != value),
        "_lt" => data.Where(x => EF.Property<int>(x!, propertyName) < value),
        "_gt" => data.Where(x => EF.Property<int>(x!, propertyName) > value),
        "_lte" => data.Where(x => EF.Property<int>(x!, propertyName) <= value),
        "_gte" => data.Where(x => EF.Property<int>(x!, propertyName) >= value),
        _ => data.Where(x => EF.Property<int>(x!, propertyName) == value),
    };

    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, string opType, string propertyName, decimal value)
    => opType switch
    {
        "_ne" => data.Where(x => EF.Property<decimal>(x!, propertyName) != value),
        "_lt" => data.Where(x => EF.Property<decimal>(x!, propertyName) < value),
        "_gt" => data.Where(x => EF.Property<decimal>(x!, propertyName) > value),
        "_lte" => data.Where(x => EF.Property<decimal>(x!, propertyName) <= value),
        "_gte" => data.Where(x => EF.Property<decimal>(x!, propertyName) >= value),
        _ => data.Where(x => EF.Property<decimal>(x!, propertyName) == value),
    };

    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, string opType, string propertyName, double value)
    => opType switch
    {
        "_ne" => data.Where(x => EF.Property<double>(x!, propertyName) != value),
        "_lt" => data.Where(x => EF.Property<double>(x!, propertyName) < value),
        "_gt" => data.Where(x => EF.Property<double>(x!, propertyName) > value),
        "_lte" => data.Where(x => EF.Property<double>(x!, propertyName) <= value),
        "_gte" => data.Where(x => EF.Property<double>(x!, propertyName) >= value),
        _ => data.Where(x => EF.Property<double>(x!, propertyName) == value),
    };

    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, string opType, string propertyName, string value)
    => opType switch
    {
        "_ne" => data.Where(x => EF.Property<string>(x!, propertyName) != value),
        _ => data.Where(x => EF.Property<string>(x!, propertyName) == value),
    };
    
    private static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> data, string opType, string propertyName, bool value)
    => opType switch
    {
        "_ne" => data.Where(x => EF.Property<bool>(x!, propertyName) != value),
        "_lt" => data.Where(x => EF.Property<bool>(x!, propertyName) != value),
        "_gt" => data.Where(x => EF.Property<bool>(x!, propertyName) != value),
        _ => data.Where(x => EF.Property<bool>(x!, propertyName) == value),
    };
}
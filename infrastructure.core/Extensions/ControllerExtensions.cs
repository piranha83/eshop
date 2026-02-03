using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using Infrastructure.Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class ControllerExtensions
{
    private static MethodInfo AddCrudControllerMethhod = typeof(ControllerExtensions).GetMethod(nameof(AddController))!;

    public static void AddControllers<TContext>(this IApplicationBuilder webApplication, JsonSerializerOptions? mapping = default)
    where TContext : DbContext
    {
        foreach (var property in typeof(TContext).GetProperties())
        {
            var propertyType = property.PropertyType;
            if (!propertyType.IsGenericType || !typeof(DbSet<>).IsAssignableFrom(propertyType.GetGenericTypeDefinition())) continue;

            var entityType = propertyType.GetGenericArguments()[0];
            AddCrudControllerMethhod
                .MakeGenericMethod(typeof(TContext), entityType, typeof(long))
                .Invoke(null, [webApplication, entityType.Name, mapping]);
        }
    }

    public static void AddController<TContext, TEntity, TKey>(
        this IEndpointRouteBuilder webApplication,
        string groupName,
        JsonSerializerOptions? mapping = null)
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    {
        var name = groupName.Replace("Model", "s").ToLower();
        var routeGroupBuilder = webApplication.MapGroup($"/{name}")
            .WithDescription($"Сощность {name}");

        routeGroupBuilder.MapGet("/", async (
            [AsParameters] [Description("Фильтры")] SearchCriteria searchCriteria,
            [FromServices] IHttpContextAccessor contextAccessor,
            [FromServices] TContext context,
            CancellationToken ct) =>
                {
                    searchCriteria.SetTerm(contextAccessor);
                    var entity = context.Set<TEntity>().AsNoTracking();
                    var data = await entity.Search<TEntity, TKey>(searchCriteria).ToListAsync(ct);
                    var total = await entity.Search<TEntity, TKey>(searchCriteria).CountAsync(ct);

                    contextAccessor.HttpContext!.Response.Headers.Append("x-total-count", total.ToString());
                    return Results.Json(data, mapping);
                })
                .Produces<List<TEntity>>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Извлечение и просмотр существующих данных");

        routeGroupBuilder.MapGet("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] TContext context,
            CancellationToken ct) =>
                Results.Json(context.Set<TEntity>().AsNoTracking().FirstOrDefault(x => x.Id.Equals(id)), mapping))
                .Produces<TEntity>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Извлечение и просмотр существующих данных");

        routeGroupBuilder.MapPost("/", async (
            [FromBody][Description("Сущность для добавления")] TEntity model,
            [FromServices] TContext context,
            CancellationToken ct) =>
                {
                    var id = await context.AddAsync(model, ct);
                    await context.SaveChangesAsync(ct);
                    return Results.Json(id, mapping);
                })
                .Produces<TKey>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Добавление новой записи в базу данных");

        routeGroupBuilder.MapPut("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromBody][Description("Сущность для обновления")] JsonPatchDocument<TEntity> patch,
            [FromServices] TContext context,
            CancellationToken ct) =>
                {
                    var entity = await context.Set<TEntity>().FirstAsync(x => x.Id.Equals(id), ct);
                    patch.ApplyTo(entity);
                    await context.SaveChangesAsync(ct);
                    return Results.Ok();
                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Изменение или модификация уже имеющихся данных");

        routeGroupBuilder.MapDelete("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] TContext context,
            CancellationToken ct) =>
                {
                    await context.Set<TEntity>().Where(x => x.Id.Equals(id)).ExecuteDeleteAsync(ct);
                    return Results.Ok();
                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Удаление записи из базы данных");
    }
}
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using Infrastructure.Core.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
    where TEntity : IEntity<TKey>
    where TKey : struct
    {
        var name = groupName.Replace("Model", "s").ToLower();
        var routeGroupBuilder = webApplication.MapGroup($"/{name}")
            .WithDescription($"Сощность {name}");

        routeGroupBuilder.MapGet("/", async (
            [AsParameters] [Description("Фильтры")] SearchCriteria searchCriteria,
            [FromServices] IHttpContextAccessor contextAccessor,
            [FromServices] IRepository<TContext, TEntity, TKey> repository,
            CancellationToken ct) =>
                {
                    searchCriteria.SetTerm(contextAccessor);
                    var data = await repository.Get(searchCriteria, ct);
                    var total = await repository.Total(searchCriteria, ct);

                    contextAccessor.HttpContext!.Response.Headers.Append("x-total-count", total.ToString());
                    return Results.Json(data, mapping);
                })
                .Produces<List<TEntity>>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Извлечение и просмотр существующих данных");

        routeGroupBuilder.MapGet("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] IRepository<TContext, TEntity, TKey> repository,
            CancellationToken ct) =>
                Results.Json(await repository.Get(id, ct), mapping))
                .Produces<TEntity>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Извлечение и просмотр существующих данных");

        routeGroupBuilder.MapPost("/", async (
            [FromBody][Description("Сущность для добавления")] TEntity model,
            [FromServices] IRepository<TContext, TEntity, TKey> repository,
            CancellationToken ct) =>
                {
                    var id = await repository.Add(model, ct);
                    await repository.SaveChanges(ct);
                    return Results.Json(id, mapping);
                })
                .Produces<TKey>()
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Добавление новой записи в базу данных");

        routeGroupBuilder.MapPut("/{id}", async (
            [FromBody][Description("Сущность для обновления")] TEntity model,
            [FromServices] IRepository<TContext, TEntity, TKey> repository,
            CancellationToken ct) =>
                {
                    await repository.Update(model, ct);
                    await repository.SaveChanges(ct);
                    return Results.Ok();
                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Изменение или модификация уже имеющихся данных");

        routeGroupBuilder.MapDelete("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] IRepository<TContext, TEntity, TKey> repository,
            CancellationToken ct) =>
                {
                    await repository.Delete(id, ct);
                    await repository.SaveChanges(ct);
                    return Results.Ok();
                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithDescription("Удаление записи из базы данных");
    }
}
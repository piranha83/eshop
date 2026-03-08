using System.ComponentModel;
using Infrastructure.Core.Abstractions;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class ControllerExtensions
{
    public static IEndpointRouteBuilder MapList<TContext, TEntity, TKey, TEntityResponse>(
        this IEndpointRouteBuilder routeGroupBuilder)
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
    {
        routeGroupBuilder.MapGet("/", async (
            [AsParameters][Description("Фильтры")] SearchCriteria searchCriteria,
            [FromServices] IEntityReadService<TContext, TEntity, TKey, TEntityResponse> entityService,
            CancellationToken ct) =>
                await entityService.Find(searchCriteria, ct) is IEnumerable<TEntityResponse> response
                    ? Results.Json(response)
                    : Results.NotFound())
                .Produces(StatusCodes.Status200OK, typeof(IEnumerable<TEntityResponse>), contentType: "application/json")
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .AllowAnonymous()
                .CacheOutput(Consts.Cache.FilterPolicy)
                .WithDescription("Извлечение и просмотр списка существующих данных");
        return routeGroupBuilder;
    }

    public static IEndpointRouteBuilder MapDetails<TContext, TEntity, TKey, TEntityResponse>(
        this IEndpointRouteBuilder routeGroupBuilder)
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
    {
        routeGroupBuilder.MapGet("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] IEntityReadService<TContext, TEntity, TKey, TEntityResponse> entityService,
            CancellationToken ct) =>
                await entityService.Find(id, ct) is TEntityResponse response ? Results.Json(response) : Results.NotFound())
                .Produces(StatusCodes.Status200OK, typeof(TEntityResponse), contentType: "application/json")
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .AllowAnonymous()
                .CacheOutput(Consts.Cache.FilterPolicy)
                .WithDescription("Извлечение и просмотр существующих данных");
        return routeGroupBuilder;
    }

    public static IEndpointRouteBuilder MapPost<TContext, TEntity, TKey, TEntityRequest>(
            this IEndpointRouteBuilder routeGroupBuilder)
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : struct
        where TEntityRequest : class, new()
    {
        routeGroupBuilder.MapPost("/", async (
                [FromBody][Description("Сущность для добавления")] TEntityRequest model,
                [FromServices] IEntityUpdateService<TContext, TEntity, TKey, TEntityRequest> entityService,
                CancellationToken ct) =>
                {
                    var id = await entityService.Add(model, ct);
                    return Results.Created(uri: $"/{id}", value: id);
                })
                    .Produces(StatusCodes.Status201Created, typeof(long), contentType: "application/json")
                    .Produces(StatusCodes.Status500InternalServerError)
                    .Produces(StatusCodes.Status401Unauthorized)
                    .Produces(StatusCodes.Status403Forbidden)
                    .ProducesValidation()
                    .RequireAuthorization(Consts.Policy.Admin)
                    .WithDescription("Добавление новой записи в базу данных");
        return routeGroupBuilder;
    }

    public static IEndpointRouteBuilder MapPut<TContext, TEntity, TKey, TEntityRequest>(
            this IEndpointRouteBuilder routeGroupBuilder)
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : struct
        where TEntityRequest : class, new()
    {
        routeGroupBuilder.MapPut("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromBody][Description("Сущность для обновления")] TEntityRequest model,
            [FromServices] IEntityUpdateService<TContext, TEntity, TKey, TEntityRequest> entityService,
            CancellationToken ct) =>
                await entityService.Update(id, model, ct) ? Results.Ok() : Results.NotFound())
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .RequireAuthorization(Consts.Policy.Admin)
                .WithDescription("Изменение или модификация уже имеющихся данных");
        return routeGroupBuilder;
    }

    public static IEndpointRouteBuilder MapDelete<TContext, TEntity, TKey>(
            this IEndpointRouteBuilder routeGroupBuilder)
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : struct
    {
        routeGroupBuilder.MapDelete("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] IEntityDeleteService<TContext, TEntity, TKey> entityService,
            CancellationToken ct) =>
                await entityService.Delete(id, ct) ? Results.Ok() : Results.NotFound())
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .RequireAuthorization(Consts.Policy.Admin)
                .WithDescription("Удаление записи из базы данных");
        return routeGroupBuilder;
    }

    private static RouteHandlerBuilder ProducesValidation(this RouteHandlerBuilder routeGroupBuilder)
    {
        return routeGroupBuilder
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .AddEndpointFilter(async (context, next) =>
        {
            try
            {
                return await next(context);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Results.ValidationProblem(new Dictionary<string, string[]>()
                {
                    { "", ["Данные устарели, необходимо обновить перед сохранением."] }
                }, statusCode: StatusCodes.Status409Conflict);
            }
            catch (ValidationApiException ex)
            {
                return Results.ValidationProblem(ex.Details, statusCode: StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}
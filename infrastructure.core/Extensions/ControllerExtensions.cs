using System.ComponentModel;
using Infrastructure.Core.Abstractions;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core.Extensions;

/// <summary>
/// Extensions.
/// </summary>
public static class ControllerExtensions
{
    public static IEndpointRouteBuilder MapGet<TContext, TEntity, TKey, TEntityResponse>(
        this IEndpointRouteBuilder routeGroupBuilder)
    where TContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : struct
    where TEntityResponse : class
    {
        routeGroupBuilder.MapGet("/", async (
            [AsParameters][Description("Фильтры")] SearchCriteria searchCriteria,
            [FromServices] IEntityReadService<TContext, TEntity, TKey, TEntityResponse> entityService,
            CancellationToken ct) => await entityService.Find(searchCriteria, ct))
                .Produces<List<TEntityResponse>>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .WithDescription("Извлечение и просмотр существующих данных");

        routeGroupBuilder.MapGet("/{id}", async (
            [FromRoute][Description("Уникальный идентификатор")] TKey id,
            [FromServices] IEntityReadService<TContext, TEntity, TKey, TEntityResponse> entityService,
            CancellationToken ct) => await entityService.Find(id, ct))
                .Produces<TEntityResponse?>(StatusCodes.Status200OK, contentType: "application/json")
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
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
                CancellationToken ct) => await entityService.Add(model, ct))
                    .Produces<TKey>()
                    .Produces(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status500InternalServerError)
                    .ProducesValidation()
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
            CancellationToken ct) => await entityService.Update(id, model, ct))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
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
            CancellationToken ct) => await entityService.Delete(id, ct))
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidation()
                .WithDescription("Удаление записи из базы данных");
        return routeGroupBuilder;
    }

    private static RouteHandlerBuilder ProducesValidation(this RouteHandlerBuilder routeGroupBuilder)
    {
        return routeGroupBuilder.Produces(StatusCodes.Status400BadRequest)
            .AddEndpointFilter(async (context, next) =>
        {
            try
            {
                return await next(context);
            }
            catch (ValidationApiException ex)
            {
                return Results.ValidationProblem(ex.Details, statusCode: StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        });
    }
}
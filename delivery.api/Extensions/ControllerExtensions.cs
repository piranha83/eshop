using System.ComponentModel;
using Delivery.Api.DatabaseContext.Models;
using Delivery.Api.Features.Address;
using Infrastructure.Core.Extensions;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.Mvc;
using Order.Api.DatabaseContext;

namespace Delivery.Api.Extensions;
/// <summary>
/// Extensions.
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Доставка.
    /// </summary>
    /// <param name="webApplication">.</param>
    public static void AddHandlers(this WebApplication webApplication)
    {
        webApplication.MapGet("address/normalize", async (
            [FromServices] IAddressClient addressClient,
            [Description("Адресс.")] string address,
            CancellationToken ct) =>
            (await addressClient.SearchAddressItemAsync(address, AddressType._2, ct)).Full_name)
        .Produces(StatusCodes.Status200OK, typeof(string), contentType: "application/json")
        .Produces(StatusCodes.Status500InternalServerError)
        .ProducesValidation()
        .AllowAnonymous()
        //.RequireAuthorization(Consts.Policy.Viewer)
        .WithDescription("Нормализация адреса.");

        webApplication.MapPost("delivery/offer/{id}", async (
            [FromServices] IEntityReadService<ApplicationDbContext, DeliveryModel, Guid, DeliveryModel> service,
            [FromRoute][Description("Уникальный идентификатор")] Guid id,
            CancellationToken ct) => Results.Json(await service.Find(id, ct)))
        .Produces(StatusCodes.Status201Created, typeof(Guid), contentType: "application/json")
        .Produces(StatusCodes.Status500InternalServerError)
        .ProducesValidation()
        .AllowAnonymous()
        //.RequireAuthorization(Consts.Policy.Admin)
        .WithDescription("Посмотреть доставку.");
    }
}
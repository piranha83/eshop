using Infrastructure.Core.Abstractions;

namespace Delivery.Api.Features.Address;

/// <summary>
/// Настройки.
/// </summary>
internal sealed class AddressOption : IClientOption
{
    /// <summary>
    /// Сервис.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Токен(sandbox).
    /// </summary>
    public string Token { get; set; } = default!;
}
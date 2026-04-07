using Infrastructure.Core.Abstractions;

namespace Payment.Api.Features.TochkaBank;

/// <summary>
/// Настройки.
/// </summary>
internal sealed class TochkaBankOptions : IClientOption
{
    /// <summary>
    /// Сервис.
    /// </summary>
    public string Url { get; set; } = default!;

    /// <summary>
    /// Токен(sandbox).
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Клиент(sandbox).
    /// </summary>
    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор ТСП в сбп.
    /// </summary>
    public string MerchantId { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор ЮР лица в сбп.
    /// </summary>
    /// <value></value>
    public string LegalId { get; set; } = default!;

    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    public string AccountId { get; set; } = default!;

    /// <summary>
    /// Уникальный код счета (AccountId без код банка).
    /// </summary>
    public string AccountCode { get; set; } = default!;

    /// <summary>
    /// Уникальный код банка.
    /// </summary>
    public string BankCode { get; set; } = default!;
}
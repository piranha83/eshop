namespace Payment.Api.Features;

/// <summary>
/// Ответ созврат.
/// </summary>
internal class RefundResponse
{
    /// <summary>
    /// Уникальный идентификатор возврата.
    /// </summary>
    public string Id { get; init; } = default!;

    /// <summary>
    /// Статус созврата.
    /// </summary>
    public string Status { get; set; } = default!;
}
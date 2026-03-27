namespace Payment.Api.Features.PaymentStatus;

/// <summary>
/// Статус.
/// </summary>
public enum PaymentStatusType
{
    /// <summary>
    /// операция не начата.
    /// </summary>
    NotStarted,

    /// <summary>
    /// операция в обработке.
    /// </summary>
    InProgress,

    /// <summary>
    /// операция завершена успешно.
    /// </summary>
    Accepted,

    /// <summary>
    /// операция отклонена.
    /// </summary>
    Rejected,

    /// <summary>
    /// возврат в обработке.
    /// </summary>
    RefundInProgress,
}

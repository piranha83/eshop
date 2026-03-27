namespace Payment.Api.Features.PaymentRefund;

/// <summary>
/// Статус.
/// </summary>
public enum PaymentRefundStatusType
{
    /// <summary>
    /// операция не начата.
    /// </summary>
    NotStarted,

    /// <summary>
    /// .
    /// </summary>
    WaitingForClientConfirm,

    /// <summary>
    /// .
    /// </summary>
    Initiated,

    /// <summary>
    /// .
    /// </summary>
    Accepted,

    /// <summary>
    /// .
    /// </summary>
    Rejected,
}
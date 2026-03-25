using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features;

/// <summary>
/// Сервис банка для сбп платежей.
/// </summary>
internal interface IBankSbpService
{
    /// <summary>
    /// Регистрация QR-кода в Системе быстрых платежей.
    /// </summary>
    /// <param name="request">Регистрация QR-кода.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>QR-код в Системе быстрых платежей.</returns>
    public Task<QrCodeResponse> GenerateQrCode(QrCodeRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить статус платежа в Системе быстрых платежей.
    /// </summary>
    /// <param name="id">Уникальный идентификатор платежа.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Статус платежа в Системе быстрых платежей.</returns>
    public Task<PaymentStatusType> GetPaymentStatus(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запрашивает возврат платежа в Системе быстрых платежей.
    /// </summary>
    /// <param name="qrCodeId">Уникальный идентификатор qr кода.</param>
    /// <param name="amount">Cумма операции в рублях.</param>
    /// <param name="currencyType">Тип валюты.</param>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>True если успех.</returns>
    public Task<RefundResponse> RefundPayment(RefundRequest request, CancellationToken cancellationToken = default);
}
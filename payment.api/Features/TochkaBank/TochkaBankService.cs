using Contract.Api.Payment;
using Microsoft.Extensions.Options;
using Payment.Api.Features.PaymentStatus;

namespace Payment.Api.Features.TochkaBank;

///<inheritdoc/>
internal sealed class TochkaBankService(
    ITochkaBankClient tochkaBankClient,
    IOptions<TochkaBankOptions> options) : IBankSbpService
{
    ///<inheritdoc/>
    public async Task<QrCodeResponse> GenerateQrCode(QrCodeRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value.MerchantId);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value.AccountId);

        var response = await tochkaBankClient.RegisterQrCodeAsync(new Body6
        {
            Data = new Data6
            {
                LegalId = options.Value.LegalId,
                MerchantId = options.Value.MerchantId,
                Amount = 1,
                Currency = CurrencyType.Rub.ToString(),
                PaymentPurpose = request.Description,
                QrcType = QrcType.Dynamic,
                CustomerCode = "Точка Песочница",
                Ttl = request.TtlMinutes,
                ImageParams = new ImageParams
                {
                    Width = 500,
                    Height = 500,
                    MediaType = "image/svg+xml",
                },
            },
        }, options.Value.MerchantId, options.Value.AccountId, cancellationToken);

        return new QrCodeResponse
        {
            Id = response.Data.QrcId,
            Payload = response.Data.Payload,
            Status = PaymentStatusType.InProgress,
        };
    }

    ///<inheritdoc/>
    public async Task<PaymentStatusType> GetPaymentStatus(string id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(id)) return PaymentStatusType.NotStarted;

        var response = await tochkaBankClient.GetQrCodePaymentStatusAsync(id, cancellationToken);

        if (response.Data.PaymentList.Count > 0)
        {
            /*в sandbox может быть несколько, берем по актуальности*/
            return response.Data.PaymentList.Where(x => x.QrcId == id)
                .Select(x => Enum.TryParse(x.Status, out PaymentStatusType paymentStatus) ? paymentStatus : PaymentStatusType.NotStarted).OrderByDescending(x => x)
                .FirstOrDefault();
        }

        return PaymentStatusType.NotStarted;
    }

    ///<inheritdoc/>
    public async Task<RefundResponse> RefundPayment(RefundRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(request.Amount, 0);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(request.QrCodeId);

        ArgumentNullException.ThrowIfNull(options.Value);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value.AccountCode);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(options.Value.BankCode);

        var response = await tochkaBankClient.StartRefundAsync(new Body7
        {
            Data = new Data7
            {
                QrcId = request.QrCodeId,
                AccountCode = options.Value.AccountCode,
                BankCode = options.Value.BankCode,
                Currency = request.Currency.ToString(),
                Amount = request.Amount,
                Purpose = request.Description,
                TrxId = request.TrxId,
                RefTransactionId = request.TransactionId,
            }
        }, cancellationToken);

        return new RefundResponse
        {
            Id = response.RequestId,
            Status = response.Status,
        };
    }
}
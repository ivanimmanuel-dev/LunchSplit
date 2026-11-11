using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchSplit.Payments;
using Square;
using Square.Payments;
using DomainPaymentStatus = LunchSplit.Payments.PaymentStatus;

namespace LunchSplit.Infrastructure;

/// <summary>Translates LunchSplit payments into Square Sandbox requests.</summary>
public class SquarePaymentService : IPaymentProcessor, IDisposable
{
    private readonly SquareClient client;
    private readonly HttpClient httpClient;
    private readonly string locationId;

    /// <summary>Creates a Square Sandbox client for the configured location.</summary>
    public SquarePaymentService(string accessToken, string locationId)
    {
        this.locationId = locationId;
        httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };

        client = new SquareClient(accessToken, new ClientOptions
        {
            BaseUrl = SquareEnvironment.Sandbox,
            HttpClient = httpClient,
            Timeout = TimeSpan.FromSeconds(5),
            MaxRetries = 0
        });
    }

    /// <summary>Creates a payment in Square Sandbox and returns its outcome.</summary>
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Amount <= 0m || decimal.Round(request.Amount, 2) != request.Amount)
        {
            return new PaymentResult(false, ErrorCode: "INVALID_AMOUNT");
        }

        // Square expects an integer amount in cents.
        long cents = checked((long)(request.Amount * 100m));

        var squareRequest = new CreatePaymentRequest
        {
            SourceId = request.CardNonce,
            IdempotencyKey = request.IdempotencyKey,
            AmountMoney = new Money
            {
                Amount = cents,
                Currency = Currency.FromCustom(request.Currency)
            },
            LocationId = locationId,
            Autocomplete = true
        };

        var response = await client.Payments.CreateAsync(squareRequest);
        var payment = response.Payment;

        if (payment is null || string.IsNullOrWhiteSpace(payment.Id))
        {
            return new PaymentResult(false, ErrorCode: "MISSING_PAYMENT");
        }

        string status = payment.Status?.ToString() ?? "";
        bool success = status == "COMPLETED" || status == "APPROVED";
        return new PaymentResult(success, payment.Id, success ? null : status);
    }

    public Task<DomainPaymentStatus> GetTransactionStatusAsync(string transactionId)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }
}

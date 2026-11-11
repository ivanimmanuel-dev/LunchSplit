using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchSplit.Payments;
using Square;
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

    public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        throw new NotImplementedException();
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

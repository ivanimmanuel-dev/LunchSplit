using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Validates payments and coordinates calls to the payment processor.</summary>
public class PaymentCoordinator
{
    private readonly IPaymentProcessor processor;

    /// <summary>Accepts the processor used to make payments.</summary>
    public PaymentCoordinator(IPaymentProcessor processor)
    {
        this.processor = processor ?? throw new ArgumentNullException(nameof(processor));
    }

    /// <summary>Validates a request before attempting payment.</summary>
    /// <exception cref="PaymentGatewayTimeoutException">
    /// The processor encounters a connection failure or timeout.
    /// </exception>
    public async Task<PaymentResult> PayAsync(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Amount <= 0m)
        {
            return new PaymentResult(false, ErrorCode: "INVALID_AMOUNT");
        }

        try
        {
            return await processor.ProcessPaymentAsync(request);
        }
        catch (HttpRequestException exception)
        {
            throw new PaymentGatewayTimeoutException(
                "The payment gateway could not be reached.", exception);
        }
        catch (TaskCanceledException exception)
        {
            throw new PaymentGatewayTimeoutException(
                "The payment gateway request timed out.", exception);
        }
    }
}

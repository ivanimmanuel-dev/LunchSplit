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
    public Task<PaymentResult> PayAsync(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Amount <= 0m)
        {
            return Task.FromResult(new PaymentResult(false, ErrorCode: "INVALID_AMOUNT"));
        }

        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Provides payment operations without exposing a gateway SDK.</summary>
public interface IPaymentProcessor
{
    /// <summary>Attempts a payment and returns its transaction or decline details.</summary>
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);

    /// <summary>Retrieves the current status of an existing transaction.</summary>
    Task<PaymentStatus> GetTransactionStatusAsync(string transactionId);
}

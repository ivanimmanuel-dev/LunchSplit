using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Reports a payment gateway connection failure or timeout.</summary>
public class PaymentGatewayTimeoutException : Exception
{
    /// <summary>Retains the original error for diagnosis.</summary>
    public PaymentGatewayTimeoutException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

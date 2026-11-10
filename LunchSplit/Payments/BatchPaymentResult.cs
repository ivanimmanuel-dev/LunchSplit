using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Summarizes payments in the same order as their requests.</summary>
public record BatchPaymentResult(IReadOnlyList<PaymentResult> Results)
{
    /// <summary>The number of successful payments.</summary>
    public int SuccessCount => Results.Count(result => result.IsSuccess);

    /// <summary>The number of unsuccessful payments.</summary>
    public int FailureCount => Results.Count - SuccessCount;
}

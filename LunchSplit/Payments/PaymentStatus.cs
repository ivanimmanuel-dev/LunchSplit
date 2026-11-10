using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Lists payment states returned by the payment processor.</summary>
public enum PaymentStatus
{
    UNKNOWN,
    APPROVED,
    PENDING,
    COMPLETED,
    CANCELED,
    FAILED
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Contains the information needed to pay one finalized share.</summary>
/// <param name="Amount">The amount in dollars, with no fractional cents.</param>
/// <param name="Currency">The ISO currency code, such as CAD.</param>
/// <param name="IdempotencyKey">A unique key retained when retrying this payment.</param>
/// <param name="CardNonce">The payment token supplied to the gateway.</param>
public record PaymentRequest(decimal Amount, string Currency, string IdempotencyKey, string CardNonce);

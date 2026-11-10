using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Payments;

/// <summary>Reports the outcome of a payment attempt.</summary>
/// <param name="IsSuccess">Whether the gateway accepted the payment.</param>
/// <param name="TransactionId">The gateway payment ID, when available.</param>
/// <param name="ErrorCode">The application failure code, or null on success.</param>
/// <param name="HttpStatusCode">The HTTP status of a gateway rejection.</param>
/// <param name="GatewayErrorCode">The original error code returned by Square.</param>
public record PaymentResult(bool IsSuccess, string? TransactionId = null, string? ErrorCode = null, int? HttpStatusCode = null, string? GatewayErrorCode = null);

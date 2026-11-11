using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchSplit.Infrastructure;
using LunchSplit.Payments;

namespace LunchSplit.Tests.Integration;

/// <summary>Tests payment operations against Square Sandbox.</summary>
[TestClass]
[TestCategory("Tier1")]
[DoNotParallelize]
public class SquareSandboxTests
{
    [TestMethod]
    public async Task ProcessPaymentAsync_ValidNonce_ReturnsSuccessAndTransactionId()
    {
        // Arrange
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var settings = SquareSandboxSettings.Load(settingsPath);
        using var service = new SquarePaymentService(settings.AccessToken, settings.LocationId);

        var request = new PaymentRequest(1.00m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        // Act
        var result = await service.ProcessPaymentAsync(request);

        // Assert
        Assert.IsTrue(result.IsSuccess, $"Square rejected the payment: {result.ErrorCode}.");
        Assert.IsFalse(string.IsNullOrWhiteSpace(result.TransactionId));
    }
}
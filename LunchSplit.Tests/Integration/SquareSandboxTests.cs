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
    /// <summary>Verifies that a successful Sandbox payment returns a transaction ID.</summary>
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

    /// <summary>Verifies that a Sandbox card decline returns CARD_DECLINED.</summary>
    [TestMethod]
    public async Task ProcessPaymentAsync_DeclinedNonce_ReturnsCardDeclined()
    {
        // Arrange
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var settings = SquareSandboxSettings.Load(settingsPath);
        using var service = new SquarePaymentService(settings.AccessToken, settings.LocationId);

        var request = new PaymentRequest(1.00m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-declined");

        // Act
        var result = await service.ProcessPaymentAsync(request);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("CARD_DECLINED", result.ErrorCode);
    }

    /// <summary>Verifies that Square rejects EUR payments at the CAD location.</summary>
    [TestMethod]
    public async Task ProcessPaymentAsync_IncompatibleCurrency_ReturnsHttp400()
    {
        // Arrange
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var settings = SquareSandboxSettings.Load(settingsPath);
        using var service = new SquarePaymentService(settings.AccessToken, settings.LocationId);

        var request = new PaymentRequest(1.00m, "EUR", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        // Act
        var result = await service.ProcessPaymentAsync(request);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(400, result.HttpStatusCode);
    }

    /// <summary>Verifies that retrying a payment returns its original transaction ID.</summary>
    [TestMethod]
    public async Task ProcessPaymentAsync_RepeatedRequest_ReturnsSameTransactionId()
    {
        // Arrange
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var settings = SquareSandboxSettings.Load(settingsPath);
        using var service = new SquarePaymentService(settings.AccessToken, settings.LocationId);

        var request = new PaymentRequest(1.00m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        // Act
        var first = await service.ProcessPaymentAsync(request);
        var second = await service.ProcessPaymentAsync(request);

        // Assert
        Assert.IsTrue(first.IsSuccess);
        Assert.IsTrue(second.IsSuccess);
        Assert.IsFalse(string.IsNullOrWhiteSpace(first.TransactionId));
        Assert.AreEqual(first.TransactionId, second.TransactionId);
    }

    /// <summary>Verifies that a completed Sandbox payment can be looked up by its ID.</summary>
    [TestMethod]
    public async Task GetTransactionStatusAsync_CompletedPayment_ReturnsCompleted()
    {
        // Arrange
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var settings = SquareSandboxSettings.Load(settingsPath);
        using var service = new SquarePaymentService(settings.AccessToken, settings.LocationId);

        var request = new PaymentRequest(1.00m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        var payment = await service.ProcessPaymentAsync(request);
        Assert.IsTrue(payment.IsSuccess, $"Square rejected the payment: {payment.ErrorCode}.");

        var transactionId = payment.TransactionId;
        Assert.IsNotNull(transactionId);

        // Act
        var status = await service.GetTransactionStatusAsync(transactionId);

        // Assert
        Assert.AreEqual(PaymentStatus.COMPLETED, status);
    }
}

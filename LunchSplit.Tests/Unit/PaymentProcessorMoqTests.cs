using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LunchSplit.Payments;
using Moq;

namespace LunchSplit.Tests.Unit;

/// <summary>Checks payment coordination without contacting a real gateway.</summary>
[TestClass]
[TestCategory("Tier2")]
public class PaymentProcessorMoqTests
{
    /// <summary>A zero bill is rejected without calling the processor.</summary>
    [TestMethod]
    public async Task PayAsync_ZeroAmount_RejectsWithoutCallingProcessor()
    {
        // Arrange
        var bill = new Bill(0m, 0m, TipMode.None, 0m);
        var attendees = new List<Attendee>
        {
            new("Ivan", 1, true)
        };

        var shares = new Splitter().CalculateShares(bill, attendees, RoundingMode.BankersRounding);

        var request = new PaymentRequest(shares[0].Amount, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        var mock = new Mock<IPaymentProcessor>();
        var coordinator = new PaymentCoordinator(mock.Object);

        // Act
        var result = await coordinator.PayAsync(request);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("INVALID_AMOUNT", result.ErrorCode);
        mock.Verify(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Never());
    }

    /// <summary>Passes the exact payment details to the processor once.</summary>
    [TestMethod]
    public async Task PayAsync_ValidRequest_PassesExactAmountOnce()
    {
        // Arrange
        var request = new PaymentRequest(30m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        var mock = new Mock<IPaymentProcessor>();
        mock.Setup(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResult(true, "tx_amount"));

        var coordinator = new PaymentCoordinator(mock.Object);

        // Act
        var result = await coordinator.PayAsync(request);

        // Assert
        Assert.IsTrue(result.IsSuccess);

        mock.Verify(p => p.ProcessPaymentAsync(It.Is<PaymentRequest>(r => r.Amount == 30m && r.Currency == "CAD" && r.IdempotencyKey == request.IdempotencyKey && r.CardNonce == request.CardNonce)), Times.Once());

        mock.VerifyNoOtherCalls();
    }

    /// <summary>Translates a network failure into a payment gateway exception.</summary>
    [TestMethod]
    public async Task PayAsync_NetworkFailure_ThrowsPaymentGatewayTimeoutException()
    {
        // Arrange
        var request = new PaymentRequest(30m, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok");

        var networkError = new HttpRequestException("Simulated gateway timeout.");

        var mock = new Mock<IPaymentProcessor>();
        mock.Setup(p => p.ProcessPaymentAsync(request)).ThrowsAsync(networkError);

        var coordinator = new PaymentCoordinator(mock.Object);

        // Act
        Func<Task> pay = () => coordinator.PayAsync(request);

        // Assert
        var exception = await Assert.ThrowsExactlyAsync<PaymentGatewayTimeoutException>(pay);

        Assert.AreSame(networkError, exception.InnerException);
    }

    /// <summary>Processes each of four attendee shares exactly once.</summary>
    [TestMethod]
    public async Task PayBatchAsync_FourAttendees_CallsProcessorFourTimes()
    {
        // Arrange
        var bill = new Bill(100m, 0m, TipMode.None, 0m);
        var attendees = new List<Attendee>
        {
        new("Ivan", 1, true),
        new("Johan", 1, true),
        new("Kevin", 1, true),
        new("Neha", 1, true)
        };

        var shares = new Splitter().CalculateShares(bill, attendees, RoundingMode.BankersRounding);

        var requests = shares.Select(share => new PaymentRequest(share.Amount, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok")).ToList();

        var mock = new Mock<IPaymentProcessor>();
        mock.Setup(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResult(true, "tx_batch"));

        var coordinator = new PaymentCoordinator(mock.Object);

        // Act
        var summary = await coordinator.PayBatchAsync(requests);

        // Assert
        Assert.AreEqual(4, summary.SuccessCount);

        mock.Verify(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Exactly(4));

        foreach (var request in requests)
        {
            mock.Verify(p => p.ProcessPaymentAsync(request), Times.Once());
        }
    }

    /// <summary>Continues processing after the second attendee's payment declines.</summary>
    [TestMethod]
    public async Task PayBatchAsync_SecondAttendeeDeclines_ReturnsTwoSuccessesAndOneFailure()
    {
        // Arrange
        var bill = new Bill(100m, 0m, TipMode.None, 0m);
        var attendees = new List<Attendee>
        {
        new("Ivan", 1, true),
        new("Johan", 1, true),
        new("Kevin", 1, true)
        };

        var shares = new Splitter().CalculateShares(bill, attendees, RoundingMode.BankersRounding);

        var requests = shares.Select(share => new PaymentRequest(share.Amount, "CAD", Guid.NewGuid().ToString(), "cnon:card-nonce-ok")).ToList();

        var mock = new Mock<IPaymentProcessor>();
        mock.SetupSequence(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(new PaymentResult(true, "tx_first")).ReturnsAsync(new PaymentResult(false, ErrorCode: "CARD_DECLINED")).ReturnsAsync(new PaymentResult(true, "tx_third"));

        var coordinator = new PaymentCoordinator(mock.Object);

        // Act
        var summary = await coordinator.PayBatchAsync(requests);

        // Assert
        Assert.AreEqual(2, summary.SuccessCount);
        Assert.AreEqual(1, summary.FailureCount);
        Assert.IsFalse(summary.Results[1].IsSuccess);
        Assert.AreEqual("CARD_DECLINED", summary.Results[1].ErrorCode);
        Assert.AreEqual("tx_third", summary.Results[2].TransactionId);

        mock.Verify(p => p.ProcessPaymentAsync(It.IsAny<PaymentRequest>()), Times.Exactly(3));
    }
}
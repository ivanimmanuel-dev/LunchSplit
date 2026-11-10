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
}
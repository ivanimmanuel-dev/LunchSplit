namespace LunchSplit.Tests
{
    [TestClass]
    public class SplitterTests
    {
        [TestMethod]
        public void ComputeTip_NoTipMode_ReturnsZero()
        {
            // Arrange
            var splitter = new Splitter();
            decimal subtotal = 100.00m;

            // Act
            decimal tip = splitter.ComputeTip(subtotal, TipMode.None, 0m);

            // Assert
            Assert.AreEqual(0.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_PercentTipMode_ReturnsCorrectPercent()
        {
            // Arrange
            var splitter = new Splitter();
            decimal subtotal = 120.00m;

            // Act
            decimal tip = splitter.ComputeTip(subtotal, TipMode.Percent, 15m);

            // Assert
            Assert.AreEqual(18.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_FixedTipMode_ReturnsFixedAmount()
        {
            // Arrange
            var splitter = new Splitter();
            decimal subtotal = 100.00m;

            // Act
            decimal tip = splitter.ComputeTip(subtotal, TipMode.Fixed, 15.00m);

            // Assert
            Assert.AreEqual(15.00m, tip);
        }

        [TestMethod]
        public void ComputeTip_NegativeSubtotal_ThrowsException()
        {
            // Arrange
            var splitter = new Splitter();
            decimal subtotal = -1m;

            // Act
            Action calculateTip = () => splitter.ComputeTip(subtotal, TipMode.None, 0m);

            // Assert
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(calculateTip);
        }

        [TestMethod]
        public void ComputeTip_NegativeFixedTip_ThrowsException()
        {
            // Arrange
            var splitter = new Splitter();
            decimal fixedTip = -1m;

            // Act
            Action calculateTip = () => splitter.ComputeTip(100m, TipMode.Fixed, fixedTip);

            // Assert
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(calculateTip);
        }

        [TestMethod]
        public void CalculateShares_EqualSplit_ApportionsEvenly()
        {
            // Arrange
            var bill = new Bill(90m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, true),
                new Attendee("Johan", 1, true),
                new Attendee("Kevin", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(3, shares.Count);
            Assert.AreEqual(30m, shares[0].Amount);
            Assert.AreEqual(30m, shares[1].Amount);
            Assert.AreEqual(30m, shares[2].Amount);
        }

        [TestMethod]
        public void CalculateShares_ProportionalSplit_ApportionsWeighted()
        {
            // Arrange
            var bill = new Bill(90m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 2, true),
                new Attendee("Johan", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(2, shares.Count);
            Assert.AreEqual(60m, shares[0].Amount);
            Assert.AreEqual(30m, shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_ExcludedAttendee_ApportionsZero()
        {
            // Arrange
            var bill = new Bill(100m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, true),
                new Attendee("Johan", 1, false),
                new Attendee("Kevin", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(3, shares.Count);
            Assert.AreEqual(50m, shares[0].Amount);
            Assert.AreEqual(0m, shares[1].Amount);
            Assert.AreEqual(50m, shares[2].Amount);
        }

        [TestMethod]
        public void CalculateShares_TaxAndPercentTip_ApportionsTotal()
        {
            // Arrange
            var bill = new Bill(100m, 13m, TipMode.Percent, 15m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, true),
                new Attendee("Johan", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(64m, shares[0].Amount);
            Assert.AreEqual(64m, shares[1].Amount);
            Assert.AreEqual(128m, shares[0].Amount + shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_TaxAndFixedTip_ApportionsTotal()
        {
            // Arrange
            var bill = new Bill(100m, 13m, TipMode.Fixed, 20m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, true),
                new Attendee("Johan", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(66.50m, shares[0].Amount);
            Assert.AreEqual(66.50m, shares[1].Amount);
            Assert.AreEqual(133.00m, shares[0].Amount + shares[1].Amount);
        }

        [TestMethod]
        public void CalculateShares_AllAttendeesExcluded_ThrowsException()
        {
            // Arrange
            var bill = new Bill(100m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, false),
                new Attendee("Johan", 1, false)
            };
            var splitter = new Splitter();

            // Act
            Action calculateShares = () =>
                splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(calculateShares);
        }

        [TestMethod]
        public void CalculateShares_TotalWeightsZero_ThrowsException()
        {
            // Arrange
            var bill = new Bill(100m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 0, true),
                new Attendee("Johan", 0, true)
            };
            var splitter = new Splitter();

            // Act
            Action calculateShares = () => splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.ThrowsExactly<InvalidOperationException>(calculateShares);
        }

        [TestMethod]
        public void CalculateShares_ExcludedAttendee_StaysZeroAfterReconciliation()
        {
            // Arrange
            var bill = new Bill(10m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, false),
                new Attendee("Johan", 1, true),
                new Attendee("Kevin", 1, true),
                new Attendee("Neha", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(0m, shares[0].Amount);
            Assert.AreEqual(10m, shares[0].Amount + shares[1].Amount + shares[2].Amount + shares[3].Amount);
        }

        [TestMethod]
        public void CalculateShares_ExcludedAttendee_StaysZeroWithRoundUp()
        {
            // Arrange
            var bill = new Bill(10m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, false),
                new Attendee("Johan", 1, true),
                new Attendee("Kevin", 1, true),
                new Attendee("Neha", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.RoundUp);

            // Assert
            Assert.AreEqual(0m, shares[0].Amount);
            Assert.AreEqual(10m, shares[0].Amount + shares[1].Amount + shares[2].Amount + shares[3].Amount);
        }

        [TestMethod]
        public void CalculateShares_RoundDown_KeepsTotalAndExcludedZero()
        {
            // Arrange
            var bill = new Bill(10m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, false),
                new Attendee("Johan", 1, true),
                new Attendee("Kevin", 1, true),
                new Attendee("Neha", 1, true)
            };
            var splitter = new Splitter();

            // Act
            var shares = splitter.CalculateShares(bill, attendees, RoundingMode.RoundDown);

            // Assert
            decimal total = shares[0].Amount + shares[1].Amount + shares[2].Amount + shares[3].Amount;

            Assert.AreEqual(10m, total);
            Assert.AreEqual(0m, shares[0].Amount);
        }
    }
}
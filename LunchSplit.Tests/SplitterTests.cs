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
    }
}
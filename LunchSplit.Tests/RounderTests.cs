using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Tests
{
    [TestClass]
    public class RounderTests
    {
        [TestMethod]
        public void RoundShares_NoRounding_PreservesRawDecimals()
        {
            // Arrange
            var rawShares = new List<Share>
            {
                new Share("Ivan", 1.234m),
                new Share("Johan", 4.321m)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.NoRounding);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1.234m, result[0].Amount);
            Assert.AreEqual(4.321m, result[1].Amount);
        }

        [TestMethod]
        public void RoundShares_BankersRounding_RoundsToNearestEven()
        {
            // Arrange
            var rawShares = new List<Share>
            {
                new Share("Ivan", 10.325m),
                new Share("Johan", 10.335m)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(10.32m, result[0].Amount);
            Assert.AreEqual(10.34m, result[1].Amount);
        }

        [TestMethod]
        public void RoundShares_RoundUp_AppliesCeilingToCents()
        {
            // Arrange
            var rawShares = new List<Share>
            {
                new Share("Ivan", 10.331m)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.RoundUp);

            // Assert
            Assert.AreEqual(10.34m, result[0].Amount);
        }

        [TestMethod]
        public void RoundShares_RoundDown_AppliesFloorToCents()
        {
            // Arrange
            var rawShares = new List<Share>
            {
                new Share("Ivan", 10.339m)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.RoundDown);

            // Assert
            Assert.AreEqual(10.33m, result[0].Amount);
        }

        [TestMethod]
        public void RoundShares_UnevenSplit_ReconcilesRemainder()
        {
            // Arrange
            decimal rawAmount = 10m / 3m;
            var rawShares = new List<Share>
            {
                new Share("Ivan", rawAmount),
                new Share("Johan", rawAmount),
                new Share("Kevin", rawAmount)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(3.34m, result[0].Amount);
            Assert.AreEqual(3.33m, result[1].Amount);
            Assert.AreEqual(3.33m, result[2].Amount);
            Assert.AreEqual(10.00m, result[0].Amount + result[1].Amount + result[2].Amount);
        }

        [TestMethod]
        public void RoundShares_EmptyShareCollection_ReturnsEmpty()
        {
            // Arrange
            var rawShares = new List<Share>();
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.BankersRounding);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void RoundShares_RoundUp_ReconcilesTotal()
        {
            // Arrange
            decimal rawAmount = 10m / 3m;
            var rawShares = new List<Share>
            {
                new Share("Ivan", rawAmount),
                new Share("Johan", rawAmount),
                new Share("Kevin", rawAmount)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.RoundUp);

            // Assert
            decimal total = result[0].Amount + result[1].Amount + result[2].Amount;
            Assert.AreEqual(10.00m, total);
        }

        [TestMethod]
        public void RoundShares_RoundDown_ReconcilesTotal()
        {
            // Arrange
            var rawShares = new List<Share>
            {
                new Share("Ivan", 3.333m),
                new Share("Johan", 3.333m),
                new Share("Kevin", 3.334m)
            };
            var rounder = new Rounder();

            // Act
            var result = rounder.RoundShares(rawShares, RoundingMode.RoundDown);

            // Assert
            decimal total = result[0].Amount + result[1].Amount + result[2].Amount;
            Assert.AreEqual(10.00m, total);
        }
    }
}

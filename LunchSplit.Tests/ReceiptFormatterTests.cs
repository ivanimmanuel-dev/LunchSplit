using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Tests
{
    [TestClass]
    public class ReceiptFormatterTests
    {
        [TestMethod]
        public void Format_DefaultBillReceipt_ContainsLineItems()
        {
            // Arrange
            var bill = new Bill(100m, 15m, TipMode.Percent, 20m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 1, true),
                new Attendee("Johan", 1, true)
            };
            var shares = new List<Share>
            {
                new Share("Ivan", 67.50m),
                new Share("Johan", 67.50m)
            };
            var formatter = new ReceiptFormatter();

            // Act
            string receipt = formatter.Format(bill, attendees, shares);

            // Assert
            Assert.IsTrue(receipt.Contains("Subtotal: $100.00"));
            Assert.IsTrue(receipt.Contains("Tax: $15.00"));
            Assert.IsTrue(receipt.Contains("Tip: $20.00"));
            Assert.IsTrue(receipt.Contains("Total: $135.00"));
            Assert.IsTrue(receipt.Contains("Ivan: $67.50"));
            Assert.IsTrue(receipt.Contains("Johan: $67.50"));
        }

        [TestMethod]
        public void Format_ReceiptOutput_ContainsRequiredMetadata()
        {
            // Arrange
            var bill = new Bill(10m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Kevin", 1, true)
            };
            var shares = new List<Share>
            {
                new Share("Kevin", 10m)
            };
            var formatter = new ReceiptFormatter();

            // Act
            DateTime before = DateTime.Now;
            string receipt = formatter.Format(bill, attendees, shares);
            DateTime after = DateTime.Now;

            // Assert
            Assert.IsTrue(receipt.Contains("Student: Ivan Immanuel Shaji"));
            Assert.IsTrue(receipt.Contains($"Date: {before:yyyy-MM-dd}") || receipt.Contains($"Date: {after:yyyy-MM-dd}"));
            Assert.IsTrue(receipt.Contains($"Created: {before:yyyy-MM-dd HH:mm}") || receipt.Contains($"Created: {after:yyyy-MM-dd HH:mm}"));
        }

        [TestMethod]
        public void Format_CsvReceiptExporter_MatchesSchema()
        {
            // Arrange
            // A sample bill and attendees.

            // Act
            // No CSV exporter exists yet.

            // Assert
            Assert.Fail("CSV receipt export is not implemented.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit.Tests
{
    [TestClass]
    public class BillValidatorTests
    {
        [TestMethod]
        public void Validate_CompleteBillDetails_ReturnsOk()
        {
            // Arrange
            var bill = new Bill(100m, 15m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 70, true)
            };
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_EmptyAttendeeCollection_ReturnsFail()
        {
            // Arrange
            var bill = new Bill(100m, 15m, TipMode.None, 0m);
            var attendees = new List<Attendee>();
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result.ErrorMessage));
        }

        [TestMethod]
        public void Validate_NegativeSubtotal_ReturnsFail()
        {
            // Arrange
            var bill = new Bill(-1m, 15m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 70, true)
            };
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("subtotal", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Validate_NegativeTax_ReturnsFail()
        {
            // Arrange
            var bill = new Bill(100m, -1m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", 70, true)
            };
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("tax", StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod]
        public void Validate_ZeroAttendees_ReturnsFail()
        {
            // Arrange
            var bill = new Bill(0m, 0m, TipMode.None, 0m);
            var attendees = new List<Attendee>();
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_NegativeWeight_ReturnsFail()
        {
            // Arrange
            var bill = new Bill(100m, 15m, TipMode.None, 0m);
            var attendees = new List<Attendee>
            {
                new Attendee("Ivan", -1, true)
            };
            var validator = new BillValidator();

            // Act
            var result = validator.Validate(bill, attendees);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.ErrorMessage.Contains("weight", StringComparison.OrdinalIgnoreCase));
        }
    }
}
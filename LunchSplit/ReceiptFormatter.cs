using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Creates an itemized text receipt with bill totals and attendee shares.
    /// </summary>
    public class ReceiptFormatter
    {
        /// <summary>
        /// Formats the bill and shares with the student name, date, and creation time.
        /// </summary>
        /// <param name="bill">The bill amounts and tip settings.</param>
        /// <param name="attendees">The attendees associated with the bill.</param>
        /// <param name="roundedShares">The calculated shares to display.</param>
        /// <returns>The receipt as a multiline string.</returns>
        public string Format(Bill bill, List<Attendee> attendees, List<Share> roundedShares)
        {
            var splitter = new Splitter();
            decimal tip = splitter.ComputeTip(bill.Subtotal, bill.TipMode, bill.TipInput);
            decimal total = bill.Subtotal + bill.Tax + tip;
            DateTime createdAt = DateTime.Now;

            var lines = new List<string>
            {
                $"Student: Ivan Immanuel Shaji",
                $"Date: {createdAt:yyyy-MM-dd}",
                $"Created: {createdAt:yyyy-MM-dd HH:mm:ss}",
                "",
                $"Subtotal: ${bill.Subtotal:0.00}",
                $"Tax: ${bill.Tax:0.00}",
                $"Tip: ${tip:0.00}",
                $"Total: ${total:0.00}",
                "",
                "Shares:"
            };

            foreach (var share in roundedShares)
            {
                lines.Add($"{share.Name}: ${share.Amount:0.00}");
            }

            return string.Join(Environment.NewLine, lines);
        }
    }
}

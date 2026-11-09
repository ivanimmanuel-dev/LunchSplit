using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Checks for empty attendee lists, negative bill amounts, and negative weights.
    /// </summary>
    public class BillValidator
    {
        /// <summary>
        /// Checks the inputs and reports the first validation failure.
        /// </summary>
        /// <param name="bill">The bill to validate.</param>
        /// <param name="attendees">The attendee list to validate.</param>
        /// <returns>The validation outcome and any failure message.</returns>
        public ValidationResult Validate(Bill bill, List<Attendee> attendees)
        {
            if (attendees.Count == 0)
            {
                return new ValidationResult(false, "At least one attendee is required.");
            }
            if (bill.Subtotal < 0m)
            {
                return new ValidationResult(false, "Subtotal cannot be negative.");
            }
            if (bill.Tax < 0m)
            {
                return new ValidationResult(false, "Tax cannot be negative.");
            }
            foreach (var attendee in attendees)
            {
                if (attendee.Weight < 0)
                {
                    return new ValidationResult(false, "Attendee weight cannot be negative.");
                }
            }
            return new ValidationResult(true, "");
        }
    }
}

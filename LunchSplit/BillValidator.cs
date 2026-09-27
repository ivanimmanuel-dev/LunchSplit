using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    public class BillValidator
    {
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

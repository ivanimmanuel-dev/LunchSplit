using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Stores a participant's name and settings for splitting the bill.
    /// </summary>
    /// <param name="Name">The participant's name.</param>
    /// <param name="Weight">Their relative share of the bill; 2 pays twice as much as 1.</param>
    /// <param name="Included">Whether they participate in this split.</param>
    public record Attendee(string Name, int Weight, bool Included);
}

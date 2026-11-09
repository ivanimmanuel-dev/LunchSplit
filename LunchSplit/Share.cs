using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Stores the amount assigned to one attendee.
    /// </summary>
    /// <param name="Name">The attendee's name.</param>
    /// <param name="Amount">Their assigned amount in dollars.</param>
    public record Share(string Name, decimal Amount);
}

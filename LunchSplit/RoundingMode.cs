using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Specifies how shares are rounded before penny reconciliation.
    /// </summary>
    public enum RoundingMode
    {
        /// <summary>Preserves the original decimal amounts.</summary>
        NoRounding,
        /// <summary>Rounds to the nearest cent, choosing the even cent at a midpoint.</summary>
        BankersRounding,
        /// <summary>Rounds toward positive infinity to the nearest cent.</summary>
        RoundUp,
        /// <summary>Rounds toward negative infinity to the nearest cent.</summary>
        RoundDown
    }
}

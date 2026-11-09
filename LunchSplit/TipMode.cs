using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Specifies how the tip is calculated.
    /// </summary>
    public enum TipMode
    {
        /// <summary>No tip is added.</summary>
        None,
        /// <summary>The tip is a percentage of the subtotal.</summary>
        Percent,
        /// <summary>The tip is a fixed dollar amount.</summary>
        Fixed

    }
}

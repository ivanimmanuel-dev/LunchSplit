using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Stores the bill amounts and tip settings.
    /// </summary>
    /// <param name="Subtotal">The amount before tax and tip.</param>
    /// <param name="Tax">The tax amount in dollars.</param>
    /// <param name="TipMode">How the tip is calculated.</param>
    /// <param name="TipInput">The tip percentage or fixed dollar amount.</param>
    public record Bill(decimal Subtotal, decimal Tax, TipMode TipMode, decimal TipInput);
}

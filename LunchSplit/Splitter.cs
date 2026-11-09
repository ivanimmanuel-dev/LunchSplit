using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    /// <summary>
    /// Calculates tips and divides the bill among included attendees by weight.
    /// </summary>
    public class Splitter
    {
        /// <summary>
        /// Calculates the tip using the selected mode.
        /// </summary>
        /// <param name="subtotal">The amount before tax and tip.</param>
        /// <param name="tipMode">Whether to use no tip, a percentage, or a fixed amount.</param>
        /// <param name="tipInput">The percentage or fixed dollar amount.</param>
        /// <returns>The calculated tip amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The subtotal or fixed tip amount is negative.
        /// </exception>
        public decimal ComputeTip(decimal subtotal, TipMode tipMode, decimal tipInput)
        {
            if(subtotal < 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(subtotal));
            }

            if (tipMode == TipMode.Percent)
            {
                return subtotal * tipInput / 100m;
            }

            else if (tipMode == TipMode.Fixed)
            {
                if (tipInput < 0m)
                {
                    throw new ArgumentOutOfRangeException(nameof(tipInput));
                }
                return tipInput;
            }

            return 0m;
        }

        /// <summary>
        /// Splits the total by attendee weight and rounds the shares.
        /// Excluded attendees receive zero.
        /// </summary>
        /// <param name="bill">The bill amounts and tip settings.</param>
        /// <param name="attendees">The participants and their split settings.</param>
        /// <param name="roundingMode">The rounding rule applied to the shares.</param>
        /// <returns>One share per attendee, in the original order.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The subtotal or fixed tip amount is negative, or the rounding mode is unsupported.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// The total weight of included attendees is zero or negative.
        /// </exception>
        public List<Share> CalculateShares(Bill bill, List<Attendee> attendees, RoundingMode roundingMode)
        {
            decimal tip = ComputeTip(bill.Subtotal, bill.TipMode, bill.TipInput);
            decimal grandTotal = bill.Subtotal + bill.Tax + tip;

            int totalWeight = 0;

            foreach (var attendee in attendees)
            {
                if (attendee.Included)
                {
                    totalWeight = totalWeight + attendee.Weight;
                }
            }

            if (totalWeight <= 0)
            {
                throw new InvalidOperationException("At least one included attendee must have a positive weight.");
            }

            var rawShares = new List<Share>();

            foreach (var attendee in attendees)
            {
                if (attendee.Included)
                {
                    decimal amount = grandTotal * attendee.Weight / totalWeight;
                    rawShares.Add(new Share(attendee.Name, amount));
                }
                else
                {
                    rawShares.Add(new Share(attendee.Name, 0m));
                }
            }

            decimal rawTotal = 0m;
            int lastPayingIndex = -1;

            for (int i = 0; i < rawShares.Count; i++)
            {
                rawTotal = rawTotal + rawShares[i].Amount;

                if (attendees[i].Included && attendees[i].Weight > 0)
                {
                    lastPayingIndex = i;
                }
            }

            // Correct any decimal division remainder before rounding to cents.
            Share lastPayingShare = rawShares[lastPayingIndex];
            rawShares[lastPayingIndex] = new Share(lastPayingShare.Name, lastPayingShare.Amount + (grandTotal - rawTotal));

            var rounder = new Rounder();
            return rounder.RoundShares(rawShares, roundingMode);
        }
    }
}

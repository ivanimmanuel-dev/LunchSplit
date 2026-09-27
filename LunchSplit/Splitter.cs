using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    public class Splitter
    {
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

            Share lastPayingShare = rawShares[lastPayingIndex];
            rawShares[lastPayingIndex] = new Share(lastPayingShare.Name, lastPayingShare.Amount + (grandTotal - rawTotal));

            var rounder = new Rounder();
            return rounder.RoundShares(rawShares, roundingMode);
        }
    }
}

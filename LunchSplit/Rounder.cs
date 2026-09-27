using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunchSplit
{
    public class Rounder
    {
        public List<Share> RoundShares(List<Share> rawShares, RoundingMode mode)
        {
            if (rawShares.Count == 0 || mode == RoundingMode.NoRounding)
            {
                return rawShares;
            }

            var roundedShares = new List<Share>();
            decimal rawTotal = 0m;
            decimal roundedTotal = 0m;

            foreach (var share in rawShares)
            {
                decimal roundedAmount = RoundAmount(share.Amount, mode);

                rawTotal = rawTotal + share.Amount;
                roundedTotal = roundedTotal + roundedAmount;
                roundedShares.Add(new Share(share.Name, roundedAmount));
            }

            decimal difference = RoundAmount(rawTotal, mode) - roundedTotal;
            int index = 0;

            while (difference != 0m)
            {
                bool canAdjust = rawShares[index].Amount > 0m && (difference > 0m || roundedShares[index].Amount >= 0.01m);

                if (canAdjust)
                {
                    decimal adjustment = 0.01m;
                    if (difference < 0m)
                    {
                        adjustment = -0.01m;
                    }

                    Share share = roundedShares[index];
                    roundedShares[index] = new Share(share.Name, share.Amount + adjustment);
                    difference = difference - adjustment;
                }

                index++;
                if (index == rawShares.Count)
                {
                    index = 0;
                }
            }

            return roundedShares;
        }

        private decimal RoundAmount(decimal amount, RoundingMode mode)
        {
            if (mode == RoundingMode.BankersRounding)
            {
                return decimal.Round(amount, 2, MidpointRounding.ToEven);
            }
            if (mode == RoundingMode.RoundUp)
            {
                return decimal.Ceiling(amount * 100m) / 100m;
            }
            if (mode == RoundingMode.RoundDown)
            {
                return decimal.Floor(amount * 100m) / 100m;
            }

            throw new ArgumentOutOfRangeException(nameof(mode));
        }
    }
}

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
            if (mode == RoundingMode.BankersRounding)
            {
                var roundedShares = new List<Share>();

                foreach (var share in rawShares)
                {
                    decimal amount = decimal.Round(share.Amount, 2, MidpointRounding.ToEven);
                    roundedShares.Add(new Share(share.Name, amount));
                }

                decimal originalTotal = 0m;
                decimal roundedTotal = 0m;

                for (int i = 0; i < rawShares.Count; i++)
                {
                    originalTotal = originalTotal + rawShares[i].Amount;
                    roundedTotal = roundedTotal + roundedShares[i].Amount;
                }

                decimal targetTotal = decimal.Round(originalTotal, 2, MidpointRounding.ToEven);
                decimal difference = targetTotal - roundedTotal;

                if (difference != 0m && roundedShares.Count > 0)
                {
                    Share firstShare = roundedShares[0];
                    roundedShares[0] = new Share(firstShare.Name, firstShare.Amount + difference);
                }

                return roundedShares;
            }

            if (mode == RoundingMode.RoundUp)
            {
                var roundedShares = new List<Share>();

                foreach (var share in rawShares)
                {
                    decimal amount = decimal.Ceiling(share.Amount * 100m) / 100m;
                    roundedShares.Add(new Share(share.Name, amount));
                }

                decimal originalTotal = 0m;
                decimal roundedTotal = 0m;

                for (int i = 0; i < rawShares.Count; i++)
                {
                    originalTotal = originalTotal + rawShares[i].Amount;
                    roundedTotal = roundedTotal + roundedShares[i].Amount;
                }

                decimal targetTotal = decimal.Ceiling(originalTotal * 100m) / 100m;
                decimal extra = roundedTotal - targetTotal;
                int index = 0;

                while (extra > 0m)
                {
                    Share share = roundedShares[index];
                    roundedShares[index] = new Share(share.Name, share.Amount - 0.01m);

                    extra = extra - 0.01m;
                    index = index + 1;
                }

                return roundedShares;
            }
            
            if (mode == RoundingMode.RoundDown)
            {
                var roundedShares = new List<Share>();

                foreach (var share in rawShares)
                {
                    decimal amount = decimal.Floor(share.Amount * 100m) / 100m;
                    roundedShares.Add(new Share(share.Name, amount));
                }

                decimal originalTotal = 0m;
                decimal roundedTotal = 0m;

                for (int i = 0; i < rawShares.Count; i++)
                {
                    originalTotal = originalTotal + rawShares[i].Amount;
                    roundedTotal = roundedTotal + roundedShares[i].Amount;
                }

                decimal targetTotal = decimal.Floor(originalTotal * 100m) / 100m;
                decimal missing = targetTotal - roundedTotal;
                int index = 0;

                while (missing > 0m)
                {
                    Share share = roundedShares[index];
                    roundedShares[index] = new Share(share.Name, share.Amount + 0.01m);

                    missing = missing - 0.01m;
                    index = index + 1;
                }

                return roundedShares;
            }


            return rawShares;
        }
    }
}

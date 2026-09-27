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
    }
}

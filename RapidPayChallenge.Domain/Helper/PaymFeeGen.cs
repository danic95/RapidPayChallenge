using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Domain.Helper
{
    public static class PaymFeeGen
    {
        private static readonly int max = 2;
        private static readonly int min = 0;

        public static decimal GenPaymFee(decimal lastFee = 0)
        {
            var randomVal = new Random(DateTime.UtcNow.Millisecond);
            var next = randomVal.NextDouble();
            var newFee = (decimal)(next * (max - min));
            return lastFee > 0 ? newFee * lastFee : newFee;
        }
    }
}

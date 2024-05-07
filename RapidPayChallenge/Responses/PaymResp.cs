using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Responses
{
    public class PaymResp
    {
        public PaymResp(string number)
        {
            CardNumber = number;
        }
        public string CardNumber { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal FeePaid { get; set; }
    }
}

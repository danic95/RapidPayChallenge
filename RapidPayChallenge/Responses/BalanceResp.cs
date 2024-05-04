using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Responses
{
    public class BalanceResp
    {
        public string Number { get; set; } = null!;
        public decimal Balance { get; set; }
    }
}

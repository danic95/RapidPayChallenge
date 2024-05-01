using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Domain.Requests
{
    public class PaymReq
    {
        public string Number { get; set; } = null!;

        public decimal Amount { get; set; }

        public string? Reference { get; set; }
    }
}

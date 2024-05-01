using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Domain.Models
{
    public class PaymFee
    {
        public Guid Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime Created { get; set; }

    }
}

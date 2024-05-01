using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Domain.Models
{
    public class Transac
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public decimal PaymFee { get; set; }

        public Guid CardId { get; set; }

        public virtual Card Card { get; set; } = null!;
    }
}

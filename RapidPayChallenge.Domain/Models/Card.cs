using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Domain.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public string CVC { get; set; }

        public decimal Balance { get; set; }

        public string AccountId { get; set; }

        public virtual Account Account { get; set; } = null!;

        public virtual ICollection<Transac> Transactions { get; set; }
    }
}

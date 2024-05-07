using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.CardMngr.DTO
{
    public class CreateCardDTO
    {
        public string Number { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public string CVC { get; set; }

        public decimal Balance { get; set; } = 0;

        public AccountDTO? Account { get; set; } = null!;
    }

    public class AccountDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

    }
}

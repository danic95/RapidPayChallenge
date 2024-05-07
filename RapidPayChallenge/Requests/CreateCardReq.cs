using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Requests
{
    public class CreateCardReq
    {
        public string Number { get; set; }

        public int ExpMonth { get; set; }

        public int ExpYear { get; set; }

        public string CVC { get; set; }

        public decimal Balance { get; set; } = 0;

        public AccountReq? Account { get; set; } = null!;
    }

    public class AccountReq
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Pass { get; set; }

    }
}

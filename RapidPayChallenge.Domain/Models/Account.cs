using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace RapidPayChallenge.Domain.Models
{
    public class Account : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = null!;
    }
}

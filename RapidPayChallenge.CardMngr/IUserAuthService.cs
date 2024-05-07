using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.CardMngr
{
    public interface IUserAuthService
    {
        public Task<(string?, DateTime?)> GetAccessToken(string Email, string Pass);
    }
}

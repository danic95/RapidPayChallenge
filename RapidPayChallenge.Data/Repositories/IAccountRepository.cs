using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.Domain.Requests;

namespace RapidPayChallenge.Data.Repositories
{
    public interface IAccountRepository
    {
        Guid CreateAccount(AccountReq req);
        Account GetAccount(string email);
    }
}

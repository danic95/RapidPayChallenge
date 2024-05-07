using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data.Repositories
{
    public interface IAccountRepository
    {
        Task<Guid> CreateAccount(Account newAccount);
        Task<Account> GetAccount(string email);
    }
}

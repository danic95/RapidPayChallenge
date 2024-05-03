using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.Domain.Requests;

namespace RapidPayChallenge.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        RapidPayDbContext context;
        public AccountRepository(RapidPayDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateAccount(AccountReq req)
        {
            var account = await GetAccount(req.Email ?? string.Empty);
            if (account != null)
            {
                return account.Id;
            }

            var newAccount = new Account
            {
                FirstName = req.FirstName ?? string.Empty,
                LastName = req.LastName ?? string.Empty,
                Email = req.Email ?? string.Empty,
                Pass = req.Pass ?? string.Empty,
            };
            context.Add(newAccount);
            context.SaveChanges();

            return newAccount.Id;
        }

        public async Task<Account> GetAccount(string email) =>
            await context.Accounts.FirstOrDefaultAsync(x => x.Email == email);

    }
}

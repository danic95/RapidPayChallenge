using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        RapidPayDbContext context;
        public AccountRepository(RapidPayDbContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateAccount(Account newAccount)
        {
            var account = await GetAccount(newAccount.Email ?? string.Empty);
            if (account != null)
            {
                return account.Id;
            }

            context.Add(newAccount);
            context.SaveChanges();

            return newAccount.Id;
        }

        public async Task<Account> GetAccount(string email) =>
            await context.Accounts.FirstOrDefaultAsync(x => x.Email == email);

    }
}

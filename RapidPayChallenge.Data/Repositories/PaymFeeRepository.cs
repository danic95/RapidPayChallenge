using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data.Repositories
{
    public class PaymFeeRepository : IPaymFeeRepository
    {
        RapidPayDbContext context;
        public PaymFeeRepository(RapidPayDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateNewPaymFee(decimal fee)
        {
            var paymentFee = new PaymFee
            {
                Fee = fee
            };
            context.Add(paymentFee);
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<(decimal currentFee, DateTime lastUpdated)> GetLastPaymFee()
        {
            var paymFee = await context.PaymFees
                                        .OrderBy(x => x.Created)
                                        .Take(1)
                                        .SingleOrDefaultAsync();
            if (paymFee == null)
            {
                return default;
            }
            return (paymFee.Fee, paymFee.Created);
        }
    }
}

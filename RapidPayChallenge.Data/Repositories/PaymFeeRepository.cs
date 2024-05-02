using System;
using System.Linq;
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

        public bool CreateNewPaymFee(decimal fee)
        {
            var paymentFee = new PaymFee
            {
                Fee = fee
            };
            context.Add(paymentFee);
            try
            {
                context.SaveChanges();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public (decimal currentFee, DateTime lastUpdated) GetLastPaymFee()
        {
            var paymentFee = context.PaymFees
                                        .OrderBy(x => x.Created)
                                        .Take(1)
                                        .SingleOrDefault();
            if (paymentFee == null)
            {
                return default;
            }
            return (paymentFee.Fee, paymentFee.Created);
        }
    }
}

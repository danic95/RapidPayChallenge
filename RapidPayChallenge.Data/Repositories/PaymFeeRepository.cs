using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

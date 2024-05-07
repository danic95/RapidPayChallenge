using System;
using System.Threading.Tasks;
using RapidPayChallenge.Data.Repositories;
using RapidPayChallenge.Domain.Helper;

namespace RapidPayChallenge.PaymFees
{
    public sealed class UFEService
    {
        private static readonly Lazy<UFEService> lazy = new(() => new UFEService());

        public static UFEService Instance { get { return lazy.Value; } }

        private UFEService()
        {
        }

        public async Task<decimal> GetPaymentFee(IPaymFeeRepository repository)
        {
            var (currFee, lastUpdated) = await repository.GetLastPaymFee();
            decimal paymFee = currFee;
            if ((lastUpdated - DateTime.UtcNow).TotalHours > 1)
            {
                var newFee = PaymFeeGen.GenPaymFee(paymFee);
                await repository.CreateNewPaymFee(newFee);
                paymFee = newFee;
            }

            return paymFee;
        }
    }
}

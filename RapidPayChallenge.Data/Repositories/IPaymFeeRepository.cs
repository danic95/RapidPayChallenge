using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Data.Repositories
{
    public interface IPaymFeeRepository
    {
        Task<bool> CreateNewPaymFee(decimal fee);

        Task<(decimal currentFee, DateTime lastUpdated)> GetLastPaymFee();
    }
}

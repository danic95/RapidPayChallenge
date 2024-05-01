using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidPayChallenge.Data.Repositories
{
    public interface IPaymFeeRepository
    {
        bool CreateNewPaymFee(decimal fee);

        (decimal currentFee, DateTime lastUpdated) GetLastPaymFee();
    }
}

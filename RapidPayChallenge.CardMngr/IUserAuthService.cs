using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Requests;

namespace RapidPayChallenge.CardMngr
{
    public interface IUserAuthService
    {
        public (string?, DateTime?) GetAccessToken(LoginReq req);
    }
}

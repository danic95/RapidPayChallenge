using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidPayChallenge.Responses
{
    public class LoginResp
    {
        public string Token { get; set; } = null!;
        public DateTime? ExpiresAt { get; set; } = null!;
    }
}

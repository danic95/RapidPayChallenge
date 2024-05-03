using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Requests;
using RapidPayChallenge.Domain.Responses;

namespace RapidPayChallenge.CardMngr
{
    public interface ICardMngrService
    {
        public Task<CreateCardResp> CreateNewCard(CreateCardReq req);

        Task<PaymResp> ProcessPayment(PaymReq request);

        public Task<BalanceResp?> GetCardBalance(string cardNumber);
    }
}

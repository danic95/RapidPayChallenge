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
        public CreateCardResp CreateNewCard(CreateCardReq req);

        PaymResp ProcessPayment(PaymReq request);

        public BalanceResp? GetCardBalance(string cardNumber);
    }
}

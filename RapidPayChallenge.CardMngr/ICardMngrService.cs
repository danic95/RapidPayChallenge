using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.CardMngr
{
    public interface ICardMngrService
    {
        public Task<string> CreateNewCard(Card req);

        Task<(string, decimal, decimal)> ProcessPayment(string Number, decimal Amount);

        public Task<decimal?> GetCardBalance(string cardNumber);
    }
}

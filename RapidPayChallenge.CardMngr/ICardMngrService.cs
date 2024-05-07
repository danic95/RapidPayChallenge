using System.Threading.Tasks;
using RapidPayChallenge.CardMngr.DTO;

namespace RapidPayChallenge.CardMngr
{
    public interface ICardMngrService
    {
        public Task<string> CreateNewCard(CreateCardDTO req);

        Task<(string, decimal, decimal)> ProcessPayment(string Number, decimal Amount);

        public Task<decimal?> GetCardBalance(string cardNumber);
    }
}

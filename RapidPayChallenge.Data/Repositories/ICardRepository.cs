using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data.Repositories
{
    public interface ICardRepository
    {
        Task<string> CreateNewCard(Card req, Guid accountId);

        Task<decimal?> GetCardBalance(string cardNumber);

        Task<bool> SaveTransaction(string cardNumber, decimal amount, decimal fee, string? reference);

        Task<bool> UpdateBalance(string cardNumber, decimal amount);

        Task<Card> GetCard(string cardNumber);
    }
}

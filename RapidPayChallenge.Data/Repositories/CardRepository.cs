using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Models;

namespace RapidPayChallenge.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        RapidPayDbContext context;
        public CardRepository(RapidPayDbContext context)
        {
            this.context = context;
        }

        public async Task<string> CreateNewCard(Card newCard, Guid accountId)
        {
            await context.AddAsync(newCard);
            await context.SaveChangesAsync();

            return newCard.Number;
        }

        public async Task<decimal?> GetCardBalance(string cardNumber)
        {
            var card = await GetCard(cardNumber);
            return card.Balance;
        }

        public async Task<bool> SaveTransaction(string cardNumber, decimal payment, decimal fee, string? reference)
        {
            var card = await GetCard(cardNumber);
            if (card == null)
            {
                return false;
            }
            var payTransaction = new Transac
            {
                Amount = payment,
                PaymFee = fee,
                CardId = card.Id
            };
            context.Add(payTransaction);
            context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdateBalance(string cardNumber, decimal amount)
        {
            var card = await GetCard(cardNumber);
            if (card == null)
            {
                return false;
            }
            card.Balance -= amount;
            context.SaveChanges();
            return true;
        }

        public async Task<Card> GetCard(string cardNumber) =>
            await context.Cards.FirstOrDefaultAsync(x => x.Number == cardNumber);

    }
}

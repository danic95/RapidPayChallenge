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
            using (var dbContextTransaction = context.Database.BeginTransaction())
            {
                await context.AddAsync(newCard);
                await context.SaveChangesAsync();

                dbContextTransaction.Commit();
            }
            return newCard.Number;
        }

        public async Task<decimal?> GetCardBalance(string cardNumber)
        {
            var card = await GetCard(cardNumber);
            if (card == null)
            {
                return null;
            }
            return card.Balance;
        }

        public async Task<bool> SaveTransaction(string cardNumber, decimal payment, decimal fee)
        {
            using (var dbContextTransaction = context.Database.BeginTransaction())
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
                card.Balance -= payment + fee;
                await context.AddAsync(payTransaction);
                await context.SaveChangesAsync();

                dbContextTransaction.Commit();
            }

            return true;
        }

        public async Task<Card> GetCard(string cardNumber) =>
            await context.Cards.FirstOrDefaultAsync(x => x.Number == cardNumber);

    }
}

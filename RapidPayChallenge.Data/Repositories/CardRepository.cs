using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.Domain.Requests;
using RapidPayChallenge.Domain.Responses;

namespace RapidPayChallenge.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        RapidPayDbContext context;
        public CardRepository(RapidPayDbContext context)
        {
            this.context = context;
        }

        public async Task<CreateCardResp> CreateNewCard(CreateCardReq req, Guid accountId)
        {
            var newCard = new Card
            {
                Number = req.Number,
                ExpMonth = req.ExpMonth,
                ExpYear = req.ExpYear,
                CVC = req.CVC,
                Balance = req.Balance,
                AccountId = accountId
            };
            await context.AddAsync(newCard);
            await context.SaveChangesAsync();

            return new CreateCardResp() { Number = newCard.Number };
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

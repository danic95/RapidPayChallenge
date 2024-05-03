using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.Domain.Requests;
using RapidPayChallenge.Domain.Responses;

namespace RapidPayChallenge.Data.Repositories
{
    public interface ICardRepository
    {
        Task<CreateCardResp> CreateNewCard(CreateCardReq req, Guid accountId);

        Task<decimal?> GetCardBalance(string cardNumber);

        Task<bool> SaveTransaction(string cardNumber, decimal amount, decimal fee, string? reference);

        Task<bool> UpdateBalance(string cardNumber, decimal amount);

        Task<Card> GetCard(string cardNumber);
    }
}

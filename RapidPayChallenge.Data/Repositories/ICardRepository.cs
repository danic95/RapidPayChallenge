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
        CreateCardResp CreateNewCard(CreateCardReq req, Guid accountId);

        decimal? GetCardBalance(string cardNumber);

        bool SaveTransaction(string cardNumber, decimal amount, decimal fee, string? reference);

        bool UpdateBalance(string cardNumber, decimal amount);

        Card GetCard(string cardNumber);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.CardMngr.DTO;
using RapidPayChallenge.Data.Repositories;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.PaymFees;

namespace RapidPayChallenge.CardMngr
{
    public class CardMngrService : ICardMngrService
    {
        IAccountRepository accountRepository;
        ICardRepository cardRepository;
        IPaymFeeRepository paymFeeRepository;
        IConfiguration config;
        ILogger<CardMngrService> logger;
        private readonly string _defaultAccountEmail;

        public CardMngrService(IAccountRepository accountRepository,
        ICardRepository cardRepository,
        IPaymFeeRepository paymFeeRepository,
        IConfiguration config,
        ILogger<CardMngrService> logger)
        {
            this.accountRepository = accountRepository;
            this.cardRepository = cardRepository;
            this.paymFeeRepository = paymFeeRepository;
            this.config = config;
            this.logger = logger;
            _defaultAccountEmail = config["DefaultEmail"];
        }

        public async Task<string> CreateNewCard(CreateCardDTO req)
        {
            string accountId;
            Card card = new Card
            {
                Balance = req.Balance,
                CVC = req.CVC,
                ExpMonth = req.ExpMonth,
                ExpYear = req.ExpYear,
                Number = req.Number,
                Account = new Account()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = req.Account.Email,
                    FirstName = req.Account.FirstName,
                    LastName = req.Account.LastName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Account.Pass)
                }
            };

            if (req.Account != null)
            {
                accountId = await accountRepository.CreateAccount(card.Account);
            }
            else
            {
                var defaultAccount = await accountRepository.GetAccount(_defaultAccountEmail);
                accountId = defaultAccount != null ? defaultAccount.Id : throw new ApplicationException("Default account not found");
            }

            return await cardRepository.CreateNewCard(card, accountId);
        }

        public async Task<decimal?> GetCardBalance(string cardNumber)
        {
            decimal? balance = await cardRepository.GetCardBalance(cardNumber);
            if (balance == null)
            {
                return null;
            }
            return balance.Value;
        }

        public async Task<(string,decimal,decimal)> ProcessPayment(string Number, decimal Amount)
        {
            logger.LogInformation(
                $"Process a payment with card number {Number} and amount {Amount}");
            var currBalance = await GetCardBalance(Number) ?? throw new ArgumentException("The card has no balance");

            var feeToPay = await UFEService.Instance.GetPaymentFee(paymFeeRepository);

            var totalDiscounted = Amount + feeToPay;
            if (currBalance - totalDiscounted < 0)
            {
                throw new ArgumentException("There is not enough balance to process this payment");
            }

            await cardRepository.SaveTransaction(Number, Amount, feeToPay);

            return (Number, Amount, feeToPay);
        }
    }
}

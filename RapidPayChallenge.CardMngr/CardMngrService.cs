using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.Data.Repositories;
using RapidPayChallenge.Domain.Requests;
using RapidPayChallenge.Domain.Responses;
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

        public async Task<CreateCardResp> CreateNewCard(CreateCardReq req)
        {
            Guid accountId;

            if (req.Account != null)
            {
                accountId = await accountRepository.CreateAccount(req.Account);
            }
            else
            {
                var defaultAccount = await accountRepository.GetAccount(_defaultAccountEmail);
                accountId = defaultAccount != null ? defaultAccount.Id : throw new ApplicationException("Default account not found");
            }

            return await cardRepository.CreateNewCard(req, accountId);
        }

        public async Task<BalanceResp?> GetCardBalance(string cardNumber)
        {
            decimal? balance = await cardRepository.GetCardBalance(cardNumber);
            if (balance == null)
            {
                return null;
            }
            return new BalanceResp { Balance = balance.Value, Number = cardNumber };
        }

        public async Task<PaymResp> ProcessPayment(PaymReq req)
        {
            logger.LogInformation(
               $"Process a payment with card number {req.Number} and amount {req.Amount}");
            var currBalance = await GetCardBalance(req.Number) ?? throw new ApplicationException("The card has no balance");

            var feeToPay = await UFEService.Instance.GetPaymentFee(paymFeeRepository);

            var totalDiscounted = req.Amount + feeToPay;
            if (currBalance.Balance - totalDiscounted < 0)
            {
                throw new ApplicationException("There is not enough balance to process this payment");
            }

            var response = new PaymResp(req.Number);
            if (await cardRepository.SaveTransaction(req.Number, req.Amount, feeToPay, req.Reference))
            {
                await cardRepository.UpdateBalance(req.Number, req.Amount + feeToPay);
                response.AmountPaid = req.Amount;
                response.FeePaid = feeToPay;
            }

            return response;
        }
    }
}

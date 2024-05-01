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

        public CreateCardResp CreateNewCard(CreateCardReq req)
        {
            Guid accountId;

            if (req.Account != null)
            {
                accountId = accountRepository.CreateAccount(req.Account);
            }
            else
            {
                var defaultAccount = accountRepository.GetAccount(_defaultAccountEmail);
                accountId = defaultAccount != null
                            ?
                            defaultAccount.Id
                            :
                            throw new ApplicationException("Default account not found");
            }

            return cardRepository.CreateNewCard(req, accountId);
        }

        public BalanceResp? GetCardBalance(string cardNumber)
        {
            decimal? balance = cardRepository.GetCardBalance(cardNumber);
            if (balance == null)
            {
                return null;
            }
            return new BalanceResp { Balance = balance.Value, Number = cardNumber };
        }

        public PaymResp ProcessPayment(PaymReq req)
        {
            logger.LogInformation(
               $"Process a payment with card number {req.Number} and amount {req.Amount}");
            var currBalance = GetCardBalance(req.Number) ?? throw new ApplicationException("The card has no balance");

            var feeToPay = UFEService.Instance.GetPaymentFee(paymFeeRepository);

            var totalDiscounted = req.Amount + feeToPay;
            if (currBalance.Balance - totalDiscounted < 0)
            {
                throw new ApplicationException("There is not enough balance to process this payment");
            }

            var response = new PaymResp(req.Number);
            if (cardRepository.SaveTransaction(req.Number, req.Amount, feeToPay, req.Reference))
            {
                cardRepository.UpdateBalance(req.Number, req.Amount + feeToPay);
                response.AmountPaid = req.Amount;
                response.FeePaid = feeToPay;
            }

            return response;
        }
    }
}

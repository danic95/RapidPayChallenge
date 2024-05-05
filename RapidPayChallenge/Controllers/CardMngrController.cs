using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Domain.Models;
using RapidPayChallenge.Requests;
using RapidPayChallenge.Responses;

namespace RapidPayChallenge.Controllers
{
    [Authorize]
    [ApiController]
    public class CardMngrController : Controller
    {
        readonly ICardMngrService _cardMngrService;
        readonly ILogger<CardMngrController> _logger;

        public CardMngrController(ICardMngrService cardMngrService,
        ILogger<CardMngrController> logger)
        {
            _cardMngrService = cardMngrService;
            _logger = logger;
        }

        // POST: CardMngrController/CreateCard
        [HttpPost("CardMngrController/CreateCard")]
        public async Task<ActionResult<CreateCardResp>> CreateCard(CreateCardReq req)
        {
            try
            {
                CreateCardResp resp = new CreateCardResp()
                {
                    Number = await _cardMngrService.CreateNewCard(new Card {
                    Balance = req.Balance,
                    CVC = req.CVC,
                    ExpMonth = req.ExpMonth,
                    ExpYear = req.ExpYear,
                    Number = req.Number,
                    Account = new Account()
                    {
                        Email = req.Account.Email,
                        FirstName = req.Account.FirstName,
                        LastName = req.Account.LastName,
                        Pass = req.Account.Pass
                    }
                    })
                };
                return CreatedAtAction("Balance", new { cardNum = resp.Number }, resp);
            }
            catch (Exception ex)
            {
                var logError = $"Error creating new card. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, logError);
            }
        }

        // PUT: CardMngrController/Payment
        [HttpPut("CardMngrController/Payment")]
        public async Task<ActionResult<PaymResp>> Payment([FromBody] PaymReq req)
        {
            PaymResp response;
            try
            {
                (string num, decimal amo, decimal fee) = await _cardMngrService.ProcessPayment(req.Number, req.Amount);
                response = new PaymResp(num){ CardNumber = num,
                    AmountPaid = amo,
                    FeePaid = fee };
            }
            catch (ArgumentException ex)
            {
                var logError =
                    $"Error processing payment " +
                    $" with amount: {req.Amount}. " +
                    $"Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return StatusCode(StatusCodes.Status400BadRequest, logError);

            }
            catch (Exception ex)
            {
                var logError =
                    $"Error processing payment " +
                    $" with amount: {req.Amount}. " +
                    $"Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, logError);
            }

            return Accepted(response);
        }

        // GET: CardMngrController/Balance/12345
        [HttpGet("CardMngrController/Balance/{cardNum}")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<BalanceResp>> Balance([FromRoute] string cardNum)
        {
            try
            {
                BalanceResp card = new BalanceResp()
                {
                    Balance = (decimal)await _cardMngrService.GetCardBalance(cardNum),
                    Number = cardNum
                };
                if (card == null)
                {
                    return NotFound(new { message = $"Card number {cardNum} does not exist" });
                }

                return Ok(card);
            }
            catch (Exception ex)
            {
                var logError = $"Error retrieving card number: {cardNum}. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }
        }

    }
}

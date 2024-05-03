using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Domain.Requests;
using RapidPayChallenge.Domain.Responses;

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
                CreateCardResp resp = await _cardMngrService.CreateNewCard(req);
                return CreatedAtAction("Balance", new { cardNum = resp.Number }, resp);
            }
            catch (Exception ex)
            {
                var logError = $"Error creating new card. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }
        }

        // PUT: CardMngrController/Payment
        [HttpPut("CardMngrController/Payment")]
        public async Task<ActionResult<PaymResp>> Payment([FromBody] PaymReq req)
        {
            PaymResp response;
            try
            {
                response = await _cardMngrService.ProcessPayment(req);
            }
            catch (Exception ex)
            {
                var logError =
                    $"Error processing payment " +
                    $" with amount: {req.Amount}. " +
                    $"Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
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
                var card = await _cardMngrService.GetCardBalance(cardNum);
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

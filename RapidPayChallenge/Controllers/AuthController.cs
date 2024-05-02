using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Domain.Requests;

namespace RapidPayChallenge.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class AuthController : Controller
    {
        readonly IUserAuthService _userAuthService;
        readonly ILogger<AuthController> _logger;

        public AuthController(IUserAuthService userAuthService,
        ILogger<AuthController> logger)
        {
            _userAuthService = userAuthService;
            _logger = logger;
        }

        [HttpPost("access-token")]
        public ActionResult GetAccessToken(LoginReq req)
        {
            try
            {
                var (accessToken, expiresAt) = _userAuthService.GetAccessToken(req);
                return Ok(new { Token = accessToken, ExpiresAt = expiresAt });
            }
            catch (Exception ex)
            {
                var logError = $"Error logging in user: {req.User}. Error message: {ex.Message}";
                _logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }
        }
    }
}

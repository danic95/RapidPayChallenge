using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Domain.Requests;

namespace RapidPayChallenge.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        IUserAuthService userAuthService;
        ILogger<AuthController> logger;

        public AuthController(IUserAuthService userAuthService,
        ILogger<AuthController> logger)
        {
            this.userAuthService = userAuthService;
            this.logger = logger;
        }

        [HttpPost("access-token")]
        public ActionResult GetAccessToken(LoginReq req)
        {
            try
            {
                var (accessToken, expiresAt) = userAuthService.GetAccessToken(req);
                return Ok(new { Token = accessToken, ExpiresAt = expiresAt });
            }
            catch (Exception ex)
            {
                var logError = $"Error logging in user: {req.User}. Error message: {ex.Message}";
                logger.LogError(logError, ex);
                return this.StatusCode(StatusCodes.Status500InternalServerError, logError);
            }
        }
    }
}

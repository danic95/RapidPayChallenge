using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.Controllers;
using RapidPayChallenge.Requests;
using RapidPayChallenge.Responses;

namespace RapidPayChallenge.Tests.Controllers
{
    public class AuthControllerTests
    {
        private Mock<IUserAuthService> _userAuthServiceMock;
        private Mock<ILogger<AuthController>> _loggerMock;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _userAuthServiceMock = new Mock<IUserAuthService>();
            _loggerMock = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_userAuthServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetAccessToken_Returns_Ok_With_Valid_Credentials()
        {
            // Arrange
            var req = new LoginReq
            {
                User = "testuser",
                Pass = "testpassword"
            };

            var expectedResponse = new LoginResp()
            {
                Token = "validAccessToken",
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            var accessToken = "validAccessToken";
            var expiresAt = DateTime.UtcNow.AddHours(1);

            _userAuthServiceMock.Setup(x => x.GetAccessToken(req.User, req.Pass))
                                .ReturnsAsync((accessToken, expiresAt));

            // Act
            var result = await _controller.GetAccessToken(req);

            // Assert
            var okActionResult = result as OkObjectResult;
            Assert.IsNotNull(okActionResult);
            var model = okActionResult.Value as LoginResp;
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedResponse.Token, model.Token);
            Assert.AreEqual(expectedResponse.ExpiresAt.Value.Date, model.ExpiresAt.Value.Date);
        }

        [Test]
        public async Task GetAccessToken_Returns_InternalServerError_On_Service_Exception()
        {
            // Arrange
            var req = new LoginReq
            {
                User = "testuser",
                Pass = "testpassword"
            };

            _userAuthServiceMock.Setup(x => x.GetAccessToken(req.User, req.Pass))
                                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetAccessToken(req);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}

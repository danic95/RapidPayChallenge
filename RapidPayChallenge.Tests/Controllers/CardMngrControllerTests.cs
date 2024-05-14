using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using NUnit.Framework;
using RapidPayChallenge.CardMngr;
using RapidPayChallenge.CardMngr.DTO;
using RapidPayChallenge.Controllers;
using RapidPayChallenge.Requests;
using RapidPayChallenge.Responses;

namespace RapidPayChallenge.Tests.Controllers
{
    public class CardMngrControllerTests
    {
        private ICardMngrService _cardMngrServiceMock;
        private ILogger<CardMngrController> _loggerMock;
        private IJwtService _jwtServiceMock;
        private CardMngrController _controller;

        [SetUp]
        public void Setup()
        {
            _cardMngrServiceMock = Substitute.For<ICardMngrService>();
            _loggerMock = Substitute.For<ILogger<CardMngrController>>();
            _jwtServiceMock = Substitute.For<IJwtService>();
            _controller = new CardMngrController(_cardMngrServiceMock, _loggerMock);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "userId"),
                        new Claim(ClaimTypes.Role, "Admin") // Role-based authorization
                    }, "mock"))
                }
            };

            _jwtServiceMock.GenerateJwtToken(Arg.Any<string>(), Arg.Any<string>())
                           .Returns("validJWTToken");
        }

        [Test]
        public async Task CreateCard_Returns_CreatedAtAction()
        {
            // Arrange
            var req = new CreateCardReq
            {
                Number = "1234567890123456",
                Balance = 100,
                CVC = "123",
                ExpMonth = 12,
                ExpYear = 2024,
                Account = new AccountReq
                {
                    Email = "test@test.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Pass = "password"
                }
            };

            var cardDTO = new CreateCardDTO
            {
                Number = req.Number,
                Balance = req.Balance,
                CVC = req.CVC,
                ExpMonth = req.ExpMonth,
                ExpYear = req.ExpYear,
                Account = new AccountDTO
                {
                    Email = req.Account.Email,
                    FirstName = req.Account.FirstName,
                    LastName = req.Account.LastName,
                    Pass = req.Account.Pass
                }
            };

            _cardMngrServiceMock.CreateNewCard(cardDTO).Returns(req.Number);

            // Act
            var result = await _controller.CreateCard(req);

            // Assert
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var model = createdAtActionResult.Value as CreateCardResp;
            Assert.IsNotNull(model);
            Assert.AreEqual(req.Number, model.Number);
        }

        [Test]
        public async Task Payment_Returns_Accepted()
        {
            // Arrange
            var req = new PaymReq
            {
                Number = "1234567890123456",
                Amount = 50
            };

            var expectedResponse = new PaymResp(req.Number)
            {
                CardNumber = req.Number,
                AmountPaid = 50,
                FeePaid = 0
            };

            _cardMngrServiceMock.ProcessPayment(req.Number, req.Amount)
                                .Returns((req.Number, 50, 0));

            // Act
            var result = await _controller.Payment(req);

            // Assert
            var acceptedActionResult = result.Result as AcceptedResult;
            Assert.IsNotNull(acceptedActionResult);
            var model = acceptedActionResult.Value as PaymResp;
            Assert.IsNotNull(model);
            Assert.AreEqual(expectedResponse.CardNumber, model.CardNumber);
            Assert.AreEqual(expectedResponse.AmountPaid, model.AmountPaid);
            Assert.AreEqual(expectedResponse.FeePaid, model.FeePaid);
        }

        [Test]
        public async Task Balance_Returns_Ok()
        {
            // Arrange
            var cardNum = "1234567890123456";
            var balance = 100;

            _cardMngrServiceMock.GetCardBalance(cardNum).Returns(balance);

            // Act
            var result = await _controller.Balance(cardNum);

            // Assert
            var okActionResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okActionResult);
            var model = okActionResult.Value as BalanceResp;
            Assert.IsNotNull(model);
            Assert.AreEqual(cardNum, model.Number);
            Assert.AreEqual(balance, model.Balance);
        }

        [Test]
        public async Task CreateCard_Returns_InternalServerError_On_Service_Exception()
        {
            // Arrange
            var req = new CreateCardReq
            {
                Number = "123456789012345678910",
                Balance = 100,
                CVC = "123",
                ExpMonth = 12,
                ExpYear = 2024,
                Account = new AccountReq
                {
                    Email = "test@test.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Pass = "password"
                }
            };

            var cardDTO = new CreateCardDTO
            {
                Number = req.Number,
                Balance = req.Balance,
                CVC = req.CVC,
                ExpMonth = req.ExpMonth,
                ExpYear = req.ExpYear,
                Account = new AccountDTO
                {
                    Email = req.Account.Email,
                    FirstName = req.Account.FirstName,
                    LastName = req.Account.LastName,
                    Pass = req.Account.Pass
                }
            };

            _cardMngrServiceMock.When(x => x.CreateNewCard(cardDTO)).Do(x => { throw new Exception("Test exception"); });

            // Act
            var result = await _controller.CreateCard(req);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task Payment_Returns_BadRequest_On_Invalid_Argument_Exception()
        {
            // Arrange
            var req = new PaymReq
            {
                Number = "1234567890123456",
                Amount = -50 // Negative amount to trigger ArgumentException
            };

            _cardMngrServiceMock.When(x => x.ProcessPayment(req.Number, req.Amount)).Do(x => { throw new ArgumentException("Invalid Amount"); });

            // Act
            var result = await _controller.Payment(req);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task Payment_Returns_InternalServerError_On_Service_Exception()
        {
            // Arrange
            var req = new PaymReq
            {
                Number = "1234567890123456",
                Amount = 50
            };

            _cardMngrServiceMock.When(x => x.ProcessPayment(req.Number, req.Amount)).Do(x => { throw new Exception("Test exception"); });

            // Act
            var result = await _controller.Payment(req);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task Balance_Returns_InternalServerError_On_Service_Exception()
        {
            // Arrange
            var cardNum = "1234567890123456";

            _cardMngrServiceMock.When(x => x.GetCardBalance(cardNum)).Do(x => { throw new Exception("Test exception"); });

            // Act
            var result = await _controller.Balance(cardNum);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Test]
        public async Task Balance_Returns_NotFound_When_Card_Not_Found()
        {
            // Arrange
            var cardNum = "1234567890123456";

            _cardMngrServiceMock.GetCardBalance(cardNum)
                                .Returns((decimal?)null);

            // Act
            var result = await _controller.Balance(cardNum);

            // Assert
            var statusCodeResult = result.Result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, statusCodeResult.StatusCode);
        }
    }

    public interface IJwtService
    {
        string GenerateJwtToken(string username, string role);
    }

    // Implementación ficticia de IJwtService para los propósitos de prueba
    public class JwtService : IJwtService
    {
        public string GenerateJwtToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("supersecretkey");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

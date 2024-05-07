using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RapidPayChallenge.Data.Repositories;

namespace RapidPayChallenge.CardMngr
{
    public class UserAuthService : IUserAuthService
    {
        IAccountRepository accountRepository;
        IConfiguration config;

        public UserAuthService(IAccountRepository accountRepository, IConfiguration config)
        {
            this.accountRepository = accountRepository;
            this.config = config;
        }

        public async Task<(string?, DateTime?)> GetAccessToken(string Email, string Pass)
        {
            var account = await accountRepository.GetAccount(Email);
            if (account == null)
            {
                return (null, null);
            }

            if (account.Pass == Pass)
            {
                var issuer = config["Jwt:Issuer"];
                var audience = config["Jwt:Audience"];
                _ = int.TryParse(config["Jwt:DurationInMinutes"], out int duration);
                var key = Encoding.ASCII.GetBytes
                (config["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                    new Claim(JwtRegisteredClaimNames.Email, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                    Expires = DateTime.UtcNow.AddMinutes(duration),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return (jwtToken, tokenDescriptor.Expires);
            }

            return (null, null);
        }

    }
}

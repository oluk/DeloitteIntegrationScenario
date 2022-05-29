
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CITYAPI.Entities;
using CITYAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CITYAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User user)
        {
            var claims =  new List<Claim>(){
               new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokendescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenhandler = new JwtSecurityTokenHandler();

            var token = tokenhandler.CreateToken(tokendescriptor);

            return tokenhandler.WriteToken(token);

        }
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DatingApp.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Security
{
    public class JWTSecurity : ISecurity
    {
        private readonly IConfiguration _configuration;
        public JWTSecurity(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SecurityToken GetToken(User existingUser, JwtSecurityTokenHandler handler)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUser.Username)
            };
            //create Signature credentials
            //this code line creates a hashed key from a tokenKey saved at server, which is unique and unknown to clients. This key will be used to create hash and checkk if a request comes with a token.
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:TokenKey").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
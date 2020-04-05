using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Security
{
    public interface ISecurity
    {
         public SecurityToken GetToken(User user, JwtSecurityTokenHandler handler);
    }
}
using System.Threading.Tasks;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Models;
using DatingApp.API.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using DatingApp.API.Security;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly ISecurity _security;
        public AuthController(IAuthRepository repo, ISecurity security)
        {
            _security = security;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.userName = userForRegister.userName.ToLower();
            if (await _repo.UserExists(userForRegister.userName))
                return BadRequest("User Already Exists");
            var userToCreate = new User()
            {
                Username = userForRegister.userName
            };
            var createdUser = await _repo.Register(userToCreate, userForRegister.password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var existingUser = await _repo.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.password);
            if (existingUser == null) return Unauthorized();

            //if authorization succesfull, then return a token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token =_security.GetToken(existingUser, tokenHandler);
            
            return Ok(
                new
                {
                    token = tokenHandler.WriteToken(token)
                }
            );
        }


    }
}
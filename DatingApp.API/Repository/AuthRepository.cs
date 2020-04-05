using System;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == userName);
            if(user == null){
                return null;
            }
            if(!verifyPasswordHash(password, user.passwordHash, user.passwordSalt)){
                return null;
            }
            return user;
        }

        private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var HMAC = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {   
                var computedHash = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i< computedHash.Length; i++){
                    if(computedHash[i] != passwordHash[i]) {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var HMAC = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = HMAC.Key;
                passwordHash = HMAC.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            if(await _context.Users.AnyAsync(x => x.Username == userName))
            {
                return true;
            }
            return false;
        }
    }
}
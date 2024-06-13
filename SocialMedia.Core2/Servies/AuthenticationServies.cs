using SocialMedia.Core.Servies.Interface;
using SocialMedia.DataLayer.Context;
using SocialMedia.DataLayer.Entites;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Org.BouncyCastle.Bcpg;

namespace SocialMedia.Core.Servies
{
    public class AuthenticationServies : IAuthentication
    {
        private readonly SocialMediaContext _context;

        public AuthenticationServies(SocialMediaContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }
        public async Task AddUserAsync(UserAuthenticationForApiKey user)
        {
            await _context.userAuthenticationForApiKeys.AddAsync(user);
            _context.SaveChanges();
        }

        public async Task<UserAuthenticationForApiKey?> FindeUser(string username, string password)
        {
            return await _context.userAuthenticationForApiKeys.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }

        public async Task<string> GetJWTTokenForCookiesAnaAuthenticate(string username, string email, string key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var KEY = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KEY), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task<bool> RemoveUserAsync(int Id)
        {
            return null;
        }
    }
}

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
using Microsoft.Extensions.Configuration;
//using Org.BouncyCastle.Bcpg;

namespace SocialMedia.Core.Servies
{
    public class AuthenticationServies : IAuthentication
    {
        private readonly SocialMediaContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationServies(SocialMediaContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
                );
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim(ClaimTypes.NameIdentifier, username));
            claimsForToken.Add(new Claim(ClaimTypes.Email, email));

            var jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);

            return tokenToReturn;
        }

        public Task<bool> RemoveUserAsync(int Id)
        {
            return null;
        }
    }
}

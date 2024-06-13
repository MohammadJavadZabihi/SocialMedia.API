using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Servies;
using SocialMedia.Core.Servies.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.API.Controllers
{
    [Route("api/AuthenticationForToken")]
    [ApiController]
    public class APIAuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authentication;
        private readonly IConfiguration _configuration;
        public APIAuthenticationController(IAuthentication authentication, IConfiguration configuration)
        {
            _authentication = authentication ?? throw new ArgumentException(nameof(authentication));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody]AuthenticateRequestBodyViewModel bodyRequest)
        {
            var user = await _authentication.FindeUser(bodyRequest.UserName,
                bodyRequest.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"])
                );
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256
                );
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("userId", user.UserId.ToString()));
            claimsForToken.Add(new Claim("UserName", user.UserName.ToString()));

            var jwtSecurityToke = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.Now,
                DateTime.Now.AddHours(24),
                signingCredentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToke);
            return Ok(tokenToReturn);
        }
    }
}

using Chat.Core.Entity;
using Chat.Core.TokenModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Core.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _configuration;
        public TokenService(IOptions<JwtOptions> configuration)
        {
            _configuration = configuration;
        }

        public JsonTokenDto GenerateJwtToken(string email, ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Value.TokenSecretKey);
            var claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration.Value.ExpirationDays)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                NotBefore = DateTime.UtcNow,
                Audience = _configuration.Value.Audience,
                Issuer = _configuration.Value.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JsonTokenDto(tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value.Ticks);
        }
    }
}
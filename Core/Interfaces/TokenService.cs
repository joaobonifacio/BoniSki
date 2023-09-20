using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using Core.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Interfaces
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly SymmetricSecurityKey key;

        public TokenService(IConfiguration _config)
        {
            this.config = _config;
            this.key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));

        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };

            var creds = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptior);

            return tokenHandler.WriteToken(token);
        }
    }
}
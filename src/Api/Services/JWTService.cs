using Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class JWTService : IJWTService
    {
        public JWTService() { }

        public string GenerateJWTAsync(string userId)
        {
            var claimsJwt = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d7vC4Qj/5yzkJ9G47NRX9Xb6UvWkXZWaA23UcYMfZp4="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                "localhost:issuer",
                "localhost:audience",
                claimsJwt,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Operation failed");
            
            return token;
        }
    }
}

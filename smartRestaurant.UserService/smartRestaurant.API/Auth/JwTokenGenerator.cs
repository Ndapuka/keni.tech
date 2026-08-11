using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace smartRestaurant.API.Auth
{
    public class JwTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public JwTokenGenerator(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }
        public JwTokenGenerator(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateToken(Guid userId, string email, string userName, string role, Guid? activeCompanyId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("userName", userName),
                new Claim(ClaimTypes.Role, role)

            };

            if (activeCompanyId.HasValue)
            {
                claims.Add(
                    new Claim("companyId", activeCompanyId.Value.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

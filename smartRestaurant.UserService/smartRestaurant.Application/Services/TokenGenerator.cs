
using Microsoft.AspNetCore.WebUtilities;
using smartRestaurant.Application.ServiceContracts;
using System.Security.Cryptography;

namespace smartRestaurant.Application.Services
{
    public class TokenGenerator : ITokenGenerator
    {

        public string GenerateEmailConfirmationToken()
        {
            return GenerateSecureToken();
        }

        public string GeneratePasswordResetToken()
        {
            return GenerateSecureToken();
        }

        public string GenerateRefreshToken()
        {
            return GenerateSecureToken();
        }

        public string GenerateSecureToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return WebEncoders.Base64UrlEncode(randomNumber);
        }
    }
}

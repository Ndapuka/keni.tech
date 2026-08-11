using System;
using System.Collections.Generic;
using System.Text;

namespace smartRestaurant.Application.DTO
{
    public class AuthenticationResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string PersonName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Token { get; set; } = default!;
        public DateTime TokenExpiration { get; set; }
        public string RefreshToken { get; set; } = default!;
        public Guid? ActiveCompanyId { get; set; }
    }
}

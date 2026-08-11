using System;
using System.Collections.Generic;
using System.Text;

namespace smartRestaurant.Application.DTO
{
    public class LoginRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}

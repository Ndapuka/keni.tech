using smartRestaurant.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartRestaurant.Application.DTO
{
    public class UpdateUserRequest
    {
        public string PersonName { get; set; } = default!;
        public GenderOptions Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

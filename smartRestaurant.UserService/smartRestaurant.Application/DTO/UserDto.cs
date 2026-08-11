using smartRestaurant.Core.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace smartRestaurant.Application.DTO
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string PersonName { get; set; } = default!;
        public GenderOptions Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = default!;
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }


    }
}

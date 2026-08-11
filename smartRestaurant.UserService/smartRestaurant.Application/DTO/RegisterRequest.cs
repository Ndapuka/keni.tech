using smartRestaurant.Core.DTO;


namespace smartRestaurant.Application.DTO;

public class RegisterRequest
{
    public Guid CompanyId { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string PersonName { get; set; } = default!;
    public GenderOptions Gender { get; set; }
    public string? PhoneNumber { get; set; }
}

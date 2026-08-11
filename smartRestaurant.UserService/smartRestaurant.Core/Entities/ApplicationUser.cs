
using smartRestaurant.Core.DTO;
using BuildingBlocks.Shared.Contracts.Enums;

namespace smartRestaurant.Core.Entities;
/// <summary>
/// Define type ApplicationUser class which acts as entity model class to store user details in data store
/// </summary>
public class ApplicationUser
{
    /// <summary>
    /// Empresa atualmente ativa do utilizador (modelo multi-empresa,
    /// tipo Slack workspaces). Nullable: um utilizador pode ainda não
    /// ter nenhuma empresa, ou nenhuma selecionada como ativa.
    /// Fonte de verdade para a claim "companyId" do JWT — nunca deve
    /// ser lida do token no refresh, sempre da BD.
    /// </summary>
    public Guid? ActiveCompanyId { get; set; }

    public Guid UserID { get; set; }

    // Authentication

    public string Email { get; set; } = default!;
    public string NormalizedEmail { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string NormalizedUserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public bool EmailConfirmed { get; set; }
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

    // Personal Details
    public string PersonName { get; set; } = default!;
    public GenderOptions Gender { get; set; }
    public string? PhoneNumber { get; set; }

    // Role (enum)
    public UserRole Role { get; set; } = UserRole.Customer;

    // Account State
    public bool IsActive { get; set; } = true;

    // Audit
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();


}


using BuildingBlocks.Shared.Contracts.User.Request;
using smartRestaurant.Application.DTO;

using smartRestaurant.Core.Entities;

namespace smartRestaurant.Application.ServiceContracts;
/// <summary>
/// Contrat for user service that contains use cases for users 
/// </summary>


public interface IUsersService
{
    // REGISTO
    Task<Guid> RegisterAsync(RegisterRequest request);

    // LOGIN
    Task<AuthenticationResponse?> AuthenticateAsync(LoginRequest request);

    // CONSULTAS
    Task<UserDto?> GetByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetAllAsync();

    // GESTÃO DE CONTA

    Task DeactivateAsync(Guid userId);

    // ATUALIZAÇÕES
    Task UpdateProfileAsync(Guid userId, UpdateUserRequest request);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    Task<Guid> CreateSellerAsync(CreateSellerRequest request);
    Task<(string RefreshToken, ApplicationUser User)> RenewRefreshTokenAsync(string refreshToken);
    Task ConfirmEmailAsync(string token);

    Task ForgotPasswordAsync(string email);

    Task ResetPasswordAsync(string token, string newPassword);

    Task<Guid> CreateInternalUserAsync(InternalCreateUserRequest request);
    Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Troca a empresa ativa do utilizador, depois de validar pertença
    /// junto do CompanyService. Devolve null se o utilizador não existir
    /// ou não for membro ativo da empresa alvo — o AuthController decide
    /// o código HTTP (401/403), o serviço não lança exceção para este caso,
    /// mantendo o mesmo estilo de AuthenticateAsync.
    /// </summary>
    Task<ApplicationUser?> SwitchActiveCompanyAsync(
        Guid userId,
        Guid companyId,
        CancellationToken cancellationToken = default);


    //Task DeleteAsync(Guid userId);
}







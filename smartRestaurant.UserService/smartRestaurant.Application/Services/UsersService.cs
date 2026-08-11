using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using smartRestaurant.Application.DTO;
using smartRestaurant.Application.ServiceContracts;
using BuildingBlocks.Shared.Contracts.Enums;
using smartRestaurant.Core.DTO;
using smartRestaurant.Core.Entities;
using smartRestaurant.Core.RepositoryContracts;
using smartRestaurant.Core.UnitOfWorkContrats;
using System.Security.Cryptography;
using BuildingBlocks.Shared.Contracts.User.Request;
//using smartRestaurant.Infrastructure.Persistence

namespace smartRestaurant.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IEmailService _emailService;
    private readonly IUserTokenRepository _userTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICompanyServiceClient _companyServiceClient;
    private readonly ILogger<UsersService> _logger;


    public UsersService(IUsersRepository usersRepository, IPasswordHasher<ApplicationUser> passwordHasher, ITokenGenerator tokenGenerator, IEmailService emailService, IUserTokenRepository userTokenRepository, IUnitOfWork unitOfWork, ICompanyServiceClient companyServiceClient, ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _emailService = emailService;
        _userTokenRepository = userTokenRepository;
        _unitOfWork = unitOfWork;
        _companyServiceClient = companyServiceClient;
        _logger = logger;
    }
    //===
    //==RegisterAsync: Registers a new user by creating an ApplicationUser entity, hashing the password, and saving it to the database. Returns the UserID of the newly registered user.
    public async Task<Guid> RegisterAsync(RegisterRequest request)
    {
        //verificar se o email já existe
        if (await _usersRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }


        // Create a new ApplicationUser instance

        //inicio da transação
        await _unitOfWork.BeginTransactionAsync();


        ApplicationUser? user = null;

        string confirmationToken = string.Empty;
        try
        {
            // Create a new ApplicationUser instance
            user = new ApplicationUser
            {
                UserID = Guid.NewGuid(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpperInvariant(),

                UserName = request.Email,
                NormalizedUserName = request.Email.ToUpperInvariant(),


                PersonName = request.PersonName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Customer,
                IsActive = false,
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(user, request.Password);

            user.PasswordHash = hashedPassword;

            // Save the user to the database
            await _usersRepository.AddAsync(user);


            //create a token for email confirmation

            confirmationToken = _tokenGenerator.GenerateEmailConfirmationToken();

            var userToken = new UserToken
            {
                UserTokenId = Guid.NewGuid(),
                UserId = user.UserID,
                Token = confirmationToken,
                TokenType = TokenType.EmailConfirmation,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                IsUsed = false
            };

            // Save the token to the database

            await _userTokenRepository.CreateAsync(userToken);
            //commit the transaction if everything is successful
            await _unitOfWork.CommitAsync();
        }

        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("An error occurred while registering the user.", ex);

        }

        user = await _usersRepository.GetUserByEmailAsync(request.Email);
        var confirmationLink = $"http://localhost:4200/confirm-email?token={confirmationToken}";

        try
        {
            await _emailService.SendConfirmationEmailAsync(
                user.Email,
                user.PersonName,
                confirmationLink);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while sending the confirmation email.");
        }


        return user.UserID;

    }



    public async Task<AuthenticationResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await _usersRepository.GetForLoginAsync(request.Email);

        if (user is null)
        {
            Console.WriteLine("Utilizador não encontrado.");
            return null;
        }

        Console.WriteLine($"Utilizador encontrado: {user.Email}");
        Console.WriteLine($"Hash: {user.PasswordHash}");



        // Validar password
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

        // Gerar refresh token
        user.RefreshToken = _tokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _usersRepository.UpdateAsync(user);

        return new AuthenticationResponse
        {
            UserId = user.UserID,
            Email = user.Email,
            PersonName = user.PersonName,
            Role = user.Role.ToString(),
            RefreshToken = user.RefreshToken,
            ActiveCompanyId = user.ActiveCompanyId
        };
    }

    public async Task<ApplicationUser?> SwitchActiveCompanyAsync(
        Guid userId,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        // Valida pertença junto do CompanyService antes de tocar em
        // qualquer estado local — nunca marcar como ativa uma empresa
        // sem confirmação externa, mesmo que o pedido pareça legítimo.
        var isMember = await _companyServiceClient.IsActiveMemberAsync(
            companyId,
            userId,
            cancellationToken);

        if (!isMember)
        {
            _logger.LogWarning(
                "Tentativa de switch para empresa {CompanyId} negada: utilizador {UserId} não é membro ativo.",
                companyId,
                userId);

            return null;
        }

        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return null;

        user.ActiveCompanyId = companyId;
        user.UpdatedAt = DateTime.UtcNow;

        await _usersRepository.UpdateAsync(user);

        return user;
    }

    //consultas
    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            return null;

        return new UserDto
        {
            UserId = user.UserID,
            Email = user.Email,
            PersonName = user.PersonName,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed
        };
    }
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _usersRepository.GetAllAsync();

        return users.Select(user => new UserDto
        {
            UserId = user.UserID,
            Email = user.Email,
            PersonName = user.PersonName,
            Gender = user.Gender,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed
        });
    }

    /// <summary>
    /// GESTAO DE CONTAS 
    /// 

    public async Task DeactivateAsync(Guid userId)
    {
        await _usersRepository.DeactivateAsync(userId);
    }

    /// ATUALIZAÇÕES

    public async Task UpdateProfileAsync(Guid userId, UpdateUserRequest request)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            throw new Exception("Utilizador não encontrado.");

        user.PersonName = request.PersonName;
        user.Gender = request.Gender;
        user.PhoneNumber = request.PhoneNumber;
        user.UpdatedAt = DateTime.UtcNow;

        await _usersRepository.UpdateAsync(user);
    }
    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _usersRepository.GetByIdAsync(userId);

        if (user is null)
            throw new Exception("Utilizador não encontrado.");

        // Validar password atual
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);

        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Password atual incorreta.");

        // Atualizar password
        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _usersRepository.UpdateAsync(user);
    }
    // ============================
    // USERNAME GENERATOR
    // ============================
    private async Task<string> GenerateUserNameAsync(string personName)
    {
        var parts = personName.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string first = parts.First();
        string last = parts.Last();

        string baseUserName = $"{first}.{last}";
        string userName = baseUserName;

        int counter = 1;

        while (await _usersRepository.ExistsUserNameAsync(userName))
        {
            userName = $"{baseUserName}{counter}";
            counter++;
        }

        return userName;
    }

    public async Task<Guid> CreateSellerAsync(CreateSellerRequest request)
    {

        if (await _usersRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("Email already exists.");


        var userName = await GenerateUserNameAsync(request.PersonName);


        var seller = new ApplicationUser
        {
            UserID = Guid.NewGuid(),
            Email = request.Email,
            UserName = userName,
            PersonName = request.PersonName,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Role = UserRole.Seller, // vendedor da aplicação
            IsActive = true,
            EmailConfirmed = true, // sellers são criados internamente
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString()
        };


        seller.PasswordHash = _passwordHasher.HashPassword(seller, request.Password);


        await _usersRepository.AddAsync(seller);

        return seller.UserID;
    }
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<(string RefreshToken, ApplicationUser User)> RenewRefreshTokenAsync(string refreshToken)
    {
        var user = await _usersRepository.GetByRefreshTokenAsync(refreshToken);

        if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            throw new Exception("Invalid or expired refresh token.");

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _usersRepository.UpdateAsync(user);

        return (newRefreshToken, user);
    }

    public async Task ConfirmEmailAsync(string token)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var userToken = await _userTokenRepository.GetByTokenAsync(token);

            _logger.LogInformation("Token recebido: '{Token}'", token);

            if (userToken is null)
                throw new InvalidOperationException("Token inválido nao encontrado.");

            if (userToken.TokenType != TokenType.EmailConfirmation)
                throw new InvalidOperationException("Tipo de token inválido.");

            if (userToken.IsUsed)
                throw new InvalidOperationException("O token já foi utilizado.");

            if (userToken.ExpiresAt < DateTime.UtcNow)
                throw new InvalidOperationException("O token expirou.");

            var user = await _usersRepository.GetByIdAsync(userToken.UserId);

            if (user is null)
                throw new InvalidOperationException("Utilizador não encontrado.");

            if (user.EmailConfirmed)
                throw new InvalidOperationException("O email já foi confirmado.");

            user.EmailConfirmed = true;
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            userToken.IsUsed = true;
            userToken.UsedAt = DateTime.UtcNow;

            await _usersRepository.UpdateAsync(user);
            await _userTokenRepository.UpdateAsync(userToken);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }


    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email);
        if (user is null)
        {
            // Optionally, you can throw an exception or just return to avoid revealing whether the email exists.
            return;
        }
        var resetToken = _tokenGenerator.GeneratePasswordResetToken();

        var userToken = new UserToken
        {
            UserId = user.UserID,
            Token = resetToken,
            TokenType = TokenType.PasswordReset,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            IsUsed = false
        };

        await _userTokenRepository.CreateAsync(userToken);

        var resetLink = $"http://localhost:4200/reset-password?token={resetToken}";

        await _emailService.SendResetPasswordEmailAsync(
            user.Email,
            user.PersonName,
            resetLink);

    }

    public async Task ResetPasswordAsync(string token, string newPassword)
    {
        var userToken = await _userTokenRepository.GetByTokenAsync(token);

        if (userToken is null)
            throw new InvalidOperationException("Token inválido não encontrado.");

        if (userToken.TokenType != TokenType.PasswordReset)
            throw new InvalidOperationException("Tipo de token inválido.");

        if (userToken.IsUsed)
            throw new InvalidOperationException("Este link já foi utilizado.");

        if (userToken.ExpiresAt < DateTime.UtcNow)
            throw new InvalidOperationException("Este link expirou.");

        var user = await _usersRepository.GetByIdAsync(userToken.UserId);

        if (user is null)
            throw new InvalidOperationException("User not found.");

        user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
        user.UpdatedAt = DateTime.UtcNow;

        userToken.IsUsed = true;
        userToken.UsedAt = DateTime.UtcNow;

        await _usersRepository.UpdateAsync(user); // Add para persistir o user no Repository
        await _userTokenRepository.UpdateAsync(userToken);
    }

    public async Task<Guid> CreateInternalUserAsync(InternalCreateUserRequest request)
    {
        // Verifica se o email já existe
        if (await _usersRepository.EmailExistsAsync(request.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var user = new ApplicationUser
            {
                UserID = Guid.NewGuid(),

                Email = request.Email,
                NormalizedEmail = request.Email.ToUpperInvariant(),

                UserName = request.UserName,
                NormalizedUserName = request.UserName.ToUpperInvariant(),

                PersonName = request.PersonName,
                PhoneNumber = request.PhoneNumber,

                // Valores por defeito
                Role = UserRole.Customer,
                Gender = GenderOptions.NotSpecified,

                // Como a empresa está a ser criada através do onboarding,
                // não faz sentido bloquear o utilizador por confirmação de email.
                IsActive = true,
                EmailConfirmed = true,

                CreatedAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _usersRepository.AddAsync(user);

            await _unitOfWork.CommitAsync();

            return user.UserID;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> IsEmailAvailableAsync(string email, CancellationToken cancellationToken = default)
    {
        var exists = await _usersRepository.ExistsByEmailAsync(email, cancellationToken);
        return !exists;
    }

    //public Task DeleteAsync(Guid userId)
    //{
    //    throw new NotImplementedException();
    //}
}




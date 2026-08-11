using FluentValidation;
using Microsoft.AspNetCore.Authorization;

//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

using smartRestaurant.API.Auth;
using smartRestaurant.Application.DTO;
using smartRestaurant.Application.ServiceContracts;
using smartRestaurant.Core.RepositoryContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace smartRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")] //api/auth
public class AuthController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly JwTokenGenerator _jwtGenerator;
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<RegisterRequest> _registerValidator;
    public AuthController(IUsersService usersService, JwTokenGenerator jwtGenerator, IUsersRepository usersRepository, IValidator<RegisterRequest> registerValidator)
    {
        _usersService = usersService;
        _jwtGenerator = jwtGenerator;
        _usersRepository = usersRepository;
        _registerValidator = registerValidator;
    }

    // POST: api/auth/register
    //Endpoint for user registration use case
    [HttpPost("register")] //api/auth/register
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var validation = await _registerValidator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
        }

        var userId = await _usersService.RegisterAsync(request);

        return CreatedAtAction(nameof(GetUser), new { id = userId }, new
        {
            Message = "User registered successfully",
            UserId = userId
        });
    }

    // GET: api/auth/user/{id}
    // Apenas para devolver o user após registo
    [HttpGet("user/{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var user = await _usersService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    // POST: api/auth/login
    //Endpoint for user login use case 
    [HttpPost("login")] //api/auth/login
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var auth = await _usersService.AuthenticateAsync(request);

        if (auth is null)
            return Unauthorized(new { Message = "Invalid credentials" });

        var token = _jwtGenerator.GenerateToken(
        auth.UserId,
        auth.Email,
        auth.PersonName, // ou UserName se preferires
        auth.Role,
        auth.ActiveCompanyId
    );

        return Ok(new
        {
            Token = token,
            RefreshToken = auth.RefreshToken,
            UserId = auth.UserId,
            Email = auth.Email,
            PersonName = auth.PersonName,
            Role = auth.Role,
            ActiveCompanyId = auth.ActiveCompanyId
        });

        //return Ok(auth);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
    [FromBody] ConfirmEmailRequest request)
    {
        await _usersService.ConfirmEmailAsync(request.Token); //erro aqui

        return Ok(new
        {
            Message = "Email confirmado com sucesso."
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ResetPasswordRequest([FromBody] ForgotPasswordRequest request) //erro se uso o forgt da microsoft ou o dto
    {
        await _usersService.ForgotPasswordAsync(request.Email);

        return Ok(new
        {
            Message = "Se existir uma conta associada a este email, será enviado um link para redefinir a password."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request)
    {
        await _usersService.ResetPasswordAsync(
            request.Token,
            request.NewPassword);

        return Ok(new
        {
            Message = "Password alterada com sucesso."
        });
    }

    [HttpPost("refresh")]

    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var result = await _usersService.RenewRefreshTokenAsync(request.RefreshToken);

        var user = result.User;

        var token = _jwtGenerator.GenerateToken(
            user.UserID,
            user.Email,
            user.UserName,
            user.Role.ToString(),
            user.ActiveCompanyId
        );

        return Ok(new
        {
            Token = token,
            result.RefreshToken,
            ActiveCompanyId = user.ActiveCompanyId,
        });
    }
    /// <summary>
    /// Troca a empresa ativa do utilizador autenticado. Valida pertença
    /// junto do CompanyService antes de reemitir o JWT com a nova claim
    /// "companyId". 403 se não for membro ativo dessa empresa.
    /// </summary>
    [Authorize]
    [HttpPost("switch-company")]
    public async Task<IActionResult> SwitchCompany([FromBody] SwitchCompanyRequest request, CancellationToken cancellationToken)
    {
        var userId = GetAuthenticatedUserId();

        if (userId is null)
            return Unauthorized();

        var user = await _usersService.SwitchActiveCompanyAsync(
            userId.Value,
            request.CompanyId,
            cancellationToken);

        if (user is null)
            return Forbid();

        var token = _jwtGenerator.GenerateToken(
            user.UserID,
            user.Email,
            user.UserName,
            user.Role.ToString(),
            user.ActiveCompanyId);

        return Ok(new
        {
            Token = token,
            ActiveCompanyId = user.ActiveCompanyId
        });
    }
    // Api/Controllers/AuthhController.cs
    [HttpGet("check-email")]
    [AllowAnonymous]
    public async Task<ActionResult<CkeckEmailAvailabilityResponse>> CheckEmailAvailability(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Ok(new CkeckEmailAvailabilityResponse(true));
        }

        var isAvailable = await _usersService.IsEmailAvailableAsync(email, cancellationToken);

        return Ok(new CkeckEmailAvailabilityResponse(isAvailable));
    }
    private Guid? GetAuthenticatedUserId()
    {
        // "sub" pode chegar mapeado para ClaimTypes.NameIdentifier consoante
        // a configuração de MapInboundClaims do JwtBearer — cobre os dois.
        var claim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Guid.TryParse(claim, out var userId) ? userId : null;
    }


}


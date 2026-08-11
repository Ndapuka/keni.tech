using AutoMapper;
using CompanyService.API.Dtos.Requests.InviteUser;
using CompanyService.API.Dtos.Requests.RegisterCompany;
using CompanyService.API.Dtos.Requests.UpdateCompany;
using CompanyService.Application.Commands.InviteUser;
using CompanyService.Application.Commands.RegisterCompany;
using CompanyService.Application.Commands.RemoveUser;
using CompanyService.Application.Commands.UpdateCompany;
using CompanyService.Application.Queries.CheckCompanyMembership;
using CompanyService.Application.Queries.GetCompaniesQuery;
using CompanyService.Application.Queries.GetCompanyDashboard;
using CompanyService.Application.Queries.GetCompanyQuery;
using CompanyService.Application.Queries.GetCurrentCompany;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyService.API.Controllers;

[ApiController]
[Route("api/companies")]
[Authorize]
public sealed class CompanyController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CompanyController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] // Created
    public async Task<IActionResult> Register(
        [FromBody] RegisterCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<RegisterCompanyCommand>(request);

        var response = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { companyId = response.CompanyId },
            response);
    }

    [HttpGet("{companyId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid companyId,
        CancellationToken cancellationToken)
    {
        var query = new GetCompanyQuery(companyId);

        var response = await _sender.Send(
            query,
            cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Empresas a que o utilizador autenticado pertence (owner ou membro
    /// convidado). Alimenta o seletor de empresa no frontend.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyCompanies(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var response = await _sender.Send(
            new GetCompaniesQuery(userId),
            cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Empresa ativa do utilizador, resolvida pela claim "companyId" do JWT
    /// — não pelo ownership. Sem claim = 204, o frontend decide o routing
    /// (seletor de empresa ou wizard de criação), não é um caso de erro.
    /// </summary>
    [HttpGet("current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrent(
        CancellationToken cancellationToken)
    {
        // O ownerUserId deverá vir do utilizador autenticado
        // através do contexto de utilizador quando a autenticação
        // estiver integrada na API.
        var activeCompanyId = GetActiveCompanyIdOrNull();

        if (activeCompanyId is null)
            return NoContent();

        var userId = GetCurrentUserId();

        var response = await _sender.Send(
            new GetCurrentCompanyQuery(activeCompanyId.Value, userId),
            cancellationToken);

        return Ok(response);
    }

    [HttpGet("{companyId:guid}/dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDashboard(
    Guid companyId,
    CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var response = await _sender.Send(
            new GetCompanyDashboardQuery(
                companyId,
                userId),
            cancellationToken);

        return Ok(response);
    }



    /// <summary>
    /// Endpoint interno, service-to-service. O UserService chama isto no
    /// fluxo de switch-company, antes de reemitir o JWT, para confirmar
    /// que o utilizador é membro ativo da empresa alvo. Não expor no
    /// Ocelot ao frontend — só na rota interna entre serviços.
    /// </summary>
    [HttpGet("{companyId:guid}/members/{userId:guid}/is-active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> IsActiveMember(
        Guid companyId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var isMember = await _sender.Send(
            new CheckCompanyMembershipQuery(companyId, userId),
            cancellationToken);

        return Ok(new { IsActiveMember = isMember });
    }



    [HttpPut("{companyId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
    Guid companyId,
    [FromBody] UpdateCompanyRequest request,
    CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateCompanyCommand>(request) with
        {
            CompanyId = companyId,
            UserId = GetCurrentUserId()
        };

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("{companyId:guid}/users/invite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> InviteUser(
        Guid companyId,
        [FromBody] InviteUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<InviteUserCommand>(request) with
        {
            CompanyId = companyId,
            InvitedByUserId = GetCurrentUserId()
        };

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst("sub")?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(claim, out var userId))
        {
            throw new UnauthorizedAccessException(
                "Authenticated user identifier was not found.");
        }

        return userId;
    }
    private Guid? GetActiveCompanyIdOrNull()
    {
        var claim = User.FindFirst("companyId")?.Value;

        return Guid.TryParse(claim, out var companyId)
            ? companyId
            : null;
    }
    [HttpDelete("{companyId:guid}/users/{userId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveUser(
    Guid companyId,
    Guid userId,
    CancellationToken cancellationToken)
    {
        var command = new RemoveUserCommand
        {
            CompanyId = companyId,
            UserId = userId,
            RemovedByUserId = GetCurrentUserId()
        };

        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}
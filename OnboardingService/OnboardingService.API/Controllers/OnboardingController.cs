using MediatR;
using Microsoft.AspNetCore.Mvc;
using Onboarding.Application.Commands;
using Onboarding.Contracts.Requests;
using Onboarding.Contracts.Responses;
using BuildingBlocks.Shared.Contracts.Enums;
using Microsoft.AspNetCore.Http;

namespace Onboarding.API.Controllers;

[ApiController]
[Route("api/onboarding")]
public class OnboardingController : ControllerBase
{
    private readonly IMediator _mediator;

    public OnboardingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterCompanyResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RegisterCompanyResponse>> RegisterCompany(
        [FromBody] RegisterCompanyRequest request,
        CancellationToken ct)
    {
        var command = new RegisterCompanyCommand
        {
            IdempotencyKey = request.IdempotencyKey,
            Email = request.Email,
            Password = request.Password,
            UserName = request.UserName,
            PersonName = request.PersonName,
            PhoneNumber = request.PhoneNumber,
            CompanyName = request.CompanyName,
            BusinessType = request.BusinessType,
            Country = request.Country,
            City = request.City
        };

        var result = await _mediator.Send(command, ct);
        return Ok(result);
    }
}


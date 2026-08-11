using AutoMapper;
using CompanyService.API.Dtos.Requests.CompleteBasicInformation;
using CompanyService.API.Dtos.Requests.CompleteBranding;
using CompanyService.API.Dtos.Requests.CompleteContactInformation;
using CompanyService.API.Dtos.Requests.CompleteFiscalInformation;
using CompanyService.Application.Commands.CompleteBasicInformation;
using CompanyService.Application.Commands.CompleteBranding;
using CompanyService.Application.Commands.CompleteContactInformation;
using CompanyService.Application.Commands.CompleteFiscalInformation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyService.API.Controllers;

[ApiController]
[Route("api/companies/{companyId:guid}/wizard")]
public sealed class WizardController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public WizardController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPut("basic-information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteBasicInformation(
        Guid companyId,
        [FromBody] CompleteBasicInformationRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CompleteBasicInformationCommand>(request);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("contact-information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteContactInformation(
        Guid companyId,
        [FromBody] CompleteContactInformationRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CompleteContactInformationCommand>(request);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("fiscal-information")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteFiscalInformation(
        Guid companyId,
        [FromBody] CompleteFiscalInformationRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CompleteFiscalInformationCommand>(request);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("branding")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteBranding(
        Guid companyId,
        [FromBody] CompleteBrandingRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CompleteBrandingCommand>(request);

        await _sender.Send(
            command,
            cancellationToken);

        return NoContent();
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Application.Features.Payments.Commands.CreatePayment;
using PaymentService.Application.Features.Payments.Commands.CancelPayment;
using PaymentService.Application.Features.Payments.Commands.RefundPayment;
using PaymentService.Application.Features.Payments.Queries.GetPaymentById;

namespace PaymentService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new payment.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PaymentResponse>> CreatePayment(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePaymentCommand(request);

        var response = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetPaymentById),
            new { id = response.PaymentId },
            response);
    }

    /// <summary>
    /// Returns a payment by id.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PaymentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentResponse>> GetPaymentById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery(id);

        var payment = await _mediator.Send(query, cancellationToken);

        return Ok(payment);
    }

    /// <summary>
    /// Refunds an existing payment.
    /// </summary>
    [HttpPost("refund")]
    [ProducesResponseType(typeof(RefundResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RefundResponse>> RefundPayment(
        [FromBody] RefundPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefundPaymentCommand(request);

        var response = await _mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Cancels an existing payment.
    /// </summary>
    [HttpPost("cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelPayment(
        [FromBody] CancelPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CancelPaymentCommand(request);

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
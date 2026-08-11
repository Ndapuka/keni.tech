using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroservice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var orders = await _orderService.GetByUserIdAsync(userId);
        return Ok(orders);
    }

    [HttpGet("number/{orderNumber}")]
    public async Task<IActionResult> GetByOrderNumber(string orderNumber)
    {
        var order = await _orderService.GetByOrderNumberAsync(orderNumber);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var order = await _orderService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateOrderRequest request)
    {
        var order = await _orderService.UpdateAsync(id, request);

        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _orderService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
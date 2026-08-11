using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroservice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemsController : ControllerBase
{
    private readonly IOrderItemService _orderItemService;

    public OrderItemsController(IOrderItemService orderItemService)
    {
        _orderItemService = orderItemService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _orderItemService.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var items = await _orderItemService.GetByOrderIdAsync(orderId);

        return Ok(items);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _orderItemService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

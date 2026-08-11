using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace OrderMicroservice.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingAddressesController : ControllerBase
{
    private readonly IShippingAddressService _shippingAddressService;

    public ShippingAddressesController(IShippingAddressService shippingAddressService)
    {
        _shippingAddressService = shippingAddressService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var address = await _shippingAddressService.GetByIdAsync(id);

        if (address == null)
            return NotFound();

        return Ok(address);
    }

    [HttpGet("order/{orderId:guid}")]
    public async Task<IActionResult> GetByOrderId(Guid orderId)
    {
        var address = await _shippingAddressService.GetByOrderIdAsync(orderId);

        if (address == null)
            return NotFound();

        return Ok(address);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, ShippingAddressRequest request)
    {
        var address = await _shippingAddressService.UpdateAsync(id, request);

        if (address == null)
            return NotFound();

        return Ok(address);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _shippingAddressService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
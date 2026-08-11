using ApplicationLayer.DTOs.ProductImages;
using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ProductsMicroservice.API.Controllers;

[ApiController]
[Route("api/product-images")]
public class ProductImagesController : ControllerBase
{
    private readonly IProductImageService _productImageService;

    public ProductImagesController(IProductImageService productImageService)
    {
        _productImageService = productImageService;
    }

    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId)
    {
        var images = await _productImageService.GetImagesByProductIdAsync(productId);

        return Ok(images);
    }

    [HttpPost]
    public async Task<IActionResult> AddImage(CreateProductImageRequestDto request)
    {
        var image = await _productImageService.AddImageAsync(request);

        return Ok(image);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productImageService.DeleteImageAsync(id);

        return NoContent();
    }
}

using ApplicationLayer.DTOs.Products;
using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ProductsMicroservice.API.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllProductsAsync());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("category/{categoryId:guid}")]
    public async Task<IActionResult> GetByCategory(Guid categoryId)
    {
        return Ok(await _productService.GetProductsByCategoryAsync(categoryId));
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured()
    {
        return Ok(await _productService.GetFeaturedProductsAsync());
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        return Ok(await _productService.SearchProductsAsync(keyword));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequestDto request)
    {
        var product = await _productService.CreateProductAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductRequestDto request)
    {
        request.Id = id;

        var product = await _productService.UpdateProductAsync(request);

        return Ok(product);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteProductAsync(id);

        return NoContent();
    }
}
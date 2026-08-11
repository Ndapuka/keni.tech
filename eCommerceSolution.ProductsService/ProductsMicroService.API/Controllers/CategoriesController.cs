using ApplicationLayer.DTOs.Categories;
using ApplicationLayer.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ProductsMicroservice.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category is null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryRequestDto request)
    {
        var category = await _categoryService.CreateCategoryAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCategoryRequestDto request)
    {
        request.Id = id;

        var category = await _categoryService.UpdateCategoryAsync(request);

        return Ok(category);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _categoryService.DeleteCategoryAsync(id);

        return NoContent();
    }
}
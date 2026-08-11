using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartRestaurant.Application.DTO;
using smartRestaurant.Application.ServiceContracts;
using smartRestaurant.Application.Services;

namespace smartRestaurant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    // ADMIN ONLY
    //[Authorize(Roles = "Admin")]
    //[Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _usersService.GetAllAsync();

        return Ok(users);
    }

    // USER OR ADMIN
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();
        var role = HttpContext.Items["Role"]?.ToString();

        // User só pode ver o próprio perfil
        if (role != "Admin" && userId != id.ToString())
            return Forbid();

        var user = await _usersService.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    // USER ONLY (não admin)
    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();

        if (userId != id.ToString())
            return Forbid();

        await _usersService.UpdateProfileAsync(id, request);
        return NoContent();
    }

    // USER ONLY
    [Authorize]
    [HttpPatch("{id:guid}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        var userId = HttpContext.Items["UserId"]?.ToString();

        if (userId != id.ToString())
            return Forbid();

        await _usersService.ChangePasswordAsync(id, request.CurrentPassword, request.NewPassword);
        return NoContent();
    }

    // ADMIN ONLY
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _usersService.DeactivateAsync(id);
        return NoContent();
    }

    // ADMIN ONLY
    [Authorize(Roles = "Admin")]
    [HttpPost("sellers")]
    public async Task<IActionResult> CreateSeller([FromBody] CreateSellerRequest request)
    {
        var sellerId = await _usersService.CreateSellerAsync(request);

        return Ok(new
        {
            Message = "Seller created successfully",
            SellerId = sellerId
        });
    }


}




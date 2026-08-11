using BuildingBlocks.Shared.Contracts.User.Request;
using BuildingBlocks.Shared.Contracts.User.Response;
using Microsoft.AspNetCore.Mvc;
using smartRestaurant.Application.ServiceContracts;


namespace smartRestaurant.API.Controllers;

[ApiController]
[Route("api/internal/users")]
public class InternalUsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public InternalUsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>
    /// Creates a user from another microservice (Onboarding Service).
    /// This endpoint is NOT intended for frontend clients.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] InternalCreateUserRequest request)
    {
        var userId = await _usersService.CreateInternalUserAsync(request);

        return Ok(new InternalCreateUserResponse
        {
            UserId = userId
        });
    }

    /// <summary>
    /// Deletes a user created during onboarding compensation.
    /// Used only when Company creation fails.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _usersService.DeactivateAsync(id);

        return NoContent();
    }
}
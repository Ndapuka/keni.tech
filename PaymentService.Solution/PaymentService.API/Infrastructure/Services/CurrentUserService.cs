using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PaymentService.Core.Interfaces.Common;


namespace PaymentService.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId =>
    _httpContextAccessor.HttpContext?
        .User?
        .FindFirst(ClaimTypes.NameIdentifier)?
        .Value;

    public string? UserName =>
        _httpContextAccessor.HttpContext?
            .User?
            .Identity?
            .Name;
}
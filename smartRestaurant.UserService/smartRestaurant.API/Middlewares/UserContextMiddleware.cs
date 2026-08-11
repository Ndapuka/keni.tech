using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace smartRestaurant.API.Middlewares
{

    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;
                var userName = context.User.FindFirst("username")?.Value;

                context.Items["UserId"] = userId;
                context.Items["Email"] = email;
                context.Items["Role"] = role;
                context.Items["UserName"] = userName;
            }
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class UserContextMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserContextMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserContextMiddleware>();
        }
    }
}

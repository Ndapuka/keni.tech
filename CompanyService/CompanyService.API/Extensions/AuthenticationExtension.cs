using System.Text;
using CompanyService.API.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CompanyService.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        var jwtSettings = configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                "JWT configuration is missing.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,

                        ValidateAudience = true,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,

                        ValidAudience = jwtSettings.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Key)),

                        ClockSkew = TimeSpan.Zero
                    };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine(
                            $"JWT AUTH FAILED: {context.Exception.Message}");

                        return Task.CompletedTask;
                    },

                    OnTokenValidated = context =>
                    {
                        Console.WriteLine(
                            "JWT AUTH SUCCESS");

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        Console.WriteLine(
                            $"JWT CHALLENGE: {context.Error} - {context.ErrorDescription}");

                        return Task.CompletedTask;
                    }
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine(
                            $"JWT AUTH FAILED: {context.Exception.Message}");

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        Console.WriteLine(
                            $"JWT CHALLENGE: {context.Error} - {context.ErrorDescription}");

                        return Task.CompletedTask;
                    }
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        Console.WriteLine(
                            $"JWT TOKEN RECEIVED: {!string.IsNullOrEmpty(context.Token)}");

                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine(
                            $"JWT AUTH FAILED: {context.Exception.Message}");

                        return Task.CompletedTask;
                    },

                    OnChallenge = context =>
                    {
                        Console.WriteLine(
                            $"JWT CHALLENGE: {context.Error} - {context.ErrorDescription}");

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}

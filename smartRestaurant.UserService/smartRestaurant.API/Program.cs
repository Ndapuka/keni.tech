using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using smartRestaurant.API.Auth;
using smartRestaurant.API.Mappers;
using smartRestaurant.API.Middlewares;
using smartRestaurant.Application;
using smartRestaurant.Application.EmailSettings;
using smartRestaurant.Application.ServiceContracts;
using smartRestaurant.Application.Services;
using smartRestaurant.Core;
using smartRestaurant.Infrastructure;
using System.Text;
using System.Text.Json.Serialization;

//using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();
builder.Services.Configure<EmailSetting>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddApplication();

// Controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

//builder.Services.AddFluentValidationAutoValidation();
// AutoMapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);

// OpenAPI (.NET 8)
//builder.Services.AddOpenApi();

// Swagger (opcional, mas podes manter)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
var allowedOrigins = new[] { "http://localhost:4200" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularLocalhost", builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
        //.SetIsOriginAllowed(origin => true);// em producao remove-se
    });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddSingleton<JwTokenGenerator>();



var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();
//app.UseHttpsRedirection();
// Routing
app.UseRouting();
app.UseCors("AllowAngularLocalhost");
// Global exception middleware
app.UseExceptionHandling();

// Auth
app.UseAuthentication();

app.UseMiddleware<UserContextMiddleware>();


app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        // 👇 Alinha com o que o Scalar espera: /openapi/{documentName}.json
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.UseSwaggerUI(c =>
    {
        // Continua a ter UI clássica se quiseres
        c.SwaggerEndpoint("/openapi/v1.json", "Users API v1");
        c.RoutePrefix = "swagger";
    });
    app.MapScalarApiReference(options =>
    {
        options.Title = "Users Microservice API";
        options.Theme = ScalarTheme.Default; // opcional
    });
}

// Controllers
app.MapControllers();

app.Run();

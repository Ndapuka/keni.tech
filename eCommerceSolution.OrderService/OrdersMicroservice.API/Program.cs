using ApplicationLayer.DependencyInjections;
using DataAccessLayer.DependencyInjections;
using Microsoft.OpenApi.Models;
using OrderMicroservice.API.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();


// Application Layer
builder.Services.AddApplicationServices(builder.Configuration);

// Infrastructure Layer
builder.Services.AddInfrastructureServices(builder.Configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Order Microservice API",
        Version = "v1",
        Description = "API responsável pela gestão de encomendas."
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
        options.Title = "Products Microservice API";
        options.Theme = ScalarTheme.Default; // opcional
    });
}

// Global Exception Middleware
app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
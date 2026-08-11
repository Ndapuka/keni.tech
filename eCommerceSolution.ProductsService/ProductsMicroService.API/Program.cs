using Scalar.AspNetCore;
using ApplicationLayer;
using DataAccessLayer;
using Microsoft.OpenApi.Models;
using ProductsMicroservice.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();

// Application Layer
builder.Services.AddDataAccessLayer(builder.Configuration);

// Data Access Layer
builder.Services.AddApplicationLayer(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Products Microservice API",
        Version = "v1",
        Description = "Products Microservice for SmartRestaurant"
    });
});

var app = builder.Build();

// Configure HTTP Pipeline

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

//app.UseHttpsRedirection();

// Global Exception Handling
app.UseExceptionHandling();

app.UseAuthorization();

app.MapControllers();

app.Run();
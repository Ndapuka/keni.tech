using PaymentService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseApplicationPipeline();

app.Run();

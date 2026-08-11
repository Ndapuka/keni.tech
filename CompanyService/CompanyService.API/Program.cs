using CompanyService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Register application services
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure HTTP request pipeline
app.UseApplicationPipeline();

app.Run();

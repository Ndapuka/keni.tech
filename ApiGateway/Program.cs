using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Carrega todos os ficheiros Ocelot da pasta Configuration



Console.WriteLine(builder.Environment.ContentRootPath);

Console.WriteLine(
    Directory.Exists(Path.Combine(builder.Environment.ContentRootPath, "Configuration")));

var files = Directory.GetFiles(
    Path.Combine(builder.Environment.ContentRootPath, "Configuration"));

foreach (var file in files)
{
    Console.WriteLine(file);
}

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddOcelot("Configuration", builder.Environment);//

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

await app.UseOcelot();

app.Run();
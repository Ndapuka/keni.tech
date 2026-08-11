using Onboarding.API.Extensions;
using Onboarding.Application;
using Onboarding.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();

builder.Services.AddOnboardingApplication();

builder.Services.AddOnboardingInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseApiPipeline();

app.Run();
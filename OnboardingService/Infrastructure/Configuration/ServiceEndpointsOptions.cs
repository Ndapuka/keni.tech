namespace Onboarding.Infrastructure.Configuration;

public class ServiceEndpointsOptions
{
    public const string SectionName = "ServiceEndpoints";

    public string UserServiceBaseUrl { get; set; } = default!;
    public string CompanyServiceBaseUrl { get; set; } = default!;
}
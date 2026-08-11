using System.Net.Http.Json;
using BuildingBlocks.Shared.Contracts.Company.Requests;
using BuildingBlocks.Shared.Contracts.Company.Responses;
using Onboarding.Application.Interfaces;

namespace Onboarding.Infrastructure.Clients;

public sealed class CompanyClient : ICompanyClient
{
    private readonly HttpClient _httpClient;

    private const string CompaniesEndpoint = "api/internal/companies";

    public CompanyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<InternalCreateCompanyResponse> CreateCompanyAsync(
        InternalCreateCompanyRequest request,
        CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(
            CompaniesEndpoint,
            request,
            ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);

            throw new HttpRequestException(
                $"Company Service returned {(int)response.StatusCode}: {error}");
        }

        var body = await response.Content.ReadFromJsonAsync<InternalCreateCompanyResponse>(
            cancellationToken: ct)
            ?? throw new InvalidOperationException(
                "Company Service returned an empty response.");

        if (body.CompanyId == Guid.Empty)
        {
            throw new InvalidOperationException(
                "Company Service returned an invalid CompanyId.");
        }

        return body;
    }
}
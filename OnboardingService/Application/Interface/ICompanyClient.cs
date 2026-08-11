using BuildingBlocks.Shared.Contracts.Company.Requests;
using BuildingBlocks.Shared.Contracts.Company.Responses;

namespace Onboarding.Application.Interfaces;

public interface ICompanyClient
{
    Task<InternalCreateCompanyResponse> CreateCompanyAsync(
        InternalCreateCompanyRequest request,
        CancellationToken ct);
}
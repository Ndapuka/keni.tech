using BuildingBlocks.Shared.Contracts.Enums;
using CompanyService.Application.DTOs.Responses;
using MediatR;

namespace CompanyService.Application.Commands.RegisterCompany;

public sealed record RegisterCompanyCommand : IRequest<RegisterCompanyResponse>
{
    public Guid OwnerUserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public BusinessType BusinessType { get; init; }
    public string Country { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
}
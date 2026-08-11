namespace CompanyService.API.Dtos.Requests.CompleteBasicInformation;

public sealed class CompleteBasicInformationRequest
{
    public Guid CompanyId { get; init; }

    public string Slug { get; init; } = string.Empty;
}
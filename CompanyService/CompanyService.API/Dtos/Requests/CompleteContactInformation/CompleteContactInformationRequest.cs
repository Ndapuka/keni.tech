namespace CompanyService.API.Dtos.Requests.CompleteContactInformation;

public sealed class CompleteContactInformationRequest
{
    public Guid CompanyId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string Phone { get; init; } = string.Empty;
}
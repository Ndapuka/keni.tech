namespace CompanyService.Core.Exceptions;

public sealed class InvalidCompanyStateException
    : CompanyDomainException
{
    public InvalidCompanyStateException(string message)
        : base(message)
    {
    }
}
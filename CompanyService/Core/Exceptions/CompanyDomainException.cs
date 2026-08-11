namespace CompanyService.Core.Exceptions;

public abstract class CompanyDomainException : Exception
{
    protected CompanyDomainException(string message)
        : base(message)
    {
    }
}

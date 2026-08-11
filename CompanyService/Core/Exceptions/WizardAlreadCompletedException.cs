namespace CompanyService.Core.Exceptions;

public sealed class WizardAlreadyCompletedException
    : CompanyDomainException
{
    public WizardAlreadyCompletedException()
        : base("The company wizard has already been completed.")
    {
    }
}
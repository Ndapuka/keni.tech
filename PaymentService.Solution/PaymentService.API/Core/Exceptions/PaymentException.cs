namespace PaymentService.Core.Exceptions;

public class PaymentException : DomainException
{
    public PaymentException(string message)
        : base(message)
    {
    }
}

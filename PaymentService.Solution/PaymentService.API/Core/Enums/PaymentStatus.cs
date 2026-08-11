namespace PaymentService.Core.Enums;

public enum PaymentStatus
{
    Pending = 1,
    Processing = 2,
    Authorized = 3,
    Paid = 4,
    Failed = 5,
    Cancelled = 6,
    Refunded = 7,
    Expired = 8
}

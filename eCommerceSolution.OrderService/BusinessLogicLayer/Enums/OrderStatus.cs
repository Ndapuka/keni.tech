namespace BusinessLogicLayer.Enums;

public enum OrderStatus
{
    Pending = 1,
    Confirmed,
    Preparing,
    Ready,
    Shipped,
    Delivered,
    Cancelled
}
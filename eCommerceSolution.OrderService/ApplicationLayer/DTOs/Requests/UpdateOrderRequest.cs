using BusinessLogicLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLayer.DTOs.Requests;

public class UpdateOrderRequest
{
    [Required]
    public OrderStatus Status { get; set; }

    [Required]
    public PaymentStatus PaymentStatus { get; set; }

    public string? Notes { get; set; }
}
using AutoMapper;
using PaymentService.Application.DTOs.Requests;
using PaymentService.Application.DTOs.Responses;
using PaymentService.Core.Entities;
using PaymentService.Core.ValueObjects;

namespace PaymentService.Application.Mappings;

public sealed class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<CreatePaymentRequest, Payment>()
            .ConstructUsing(src => new Payment(
                src.OrderId,
                src.UserId,
                new Money(src.Amount, src.Currency),
                src.PaymentMethod,
                src.Provider,
                src.Description));

        CreateMap<Payment, PaymentResponse>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Amount))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency));

        CreateMap<Payment, PaymentStatusResponse>()
            .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id));
    }
}
using ApplicationLayer.DTOs.Requests;
using ApplicationLayer.DTOs.Responses;
using AutoMapper;
using BusinessLogicLayer.Entities;

namespace ApplicationLayer.Mappings;

public class OrderMappingProfile : Profile
{
    public OrderMappingProfile()
    {
        // Order
        CreateMap<CreateOrderRequest, Order>();

        CreateMap<Order, OrderResponse>();

        // OrderItem
        CreateMap<CreateOrderItemRequest, OrderItem>();

        CreateMap<OrderItem, OrderItemResponse>();

        // ShippingAddress
        CreateMap<ShippingAddressRequest, ShippingAddress>();

        CreateMap<ShippingAddress, ShippingAddressResponse>();
    }
}

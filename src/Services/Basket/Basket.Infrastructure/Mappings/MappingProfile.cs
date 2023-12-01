using AutoMapper;
using Basket.Domain.Entities;
using EventBus.Messages.Events;
using Card = Basket.Domain.Entities.Card;
using ShippingAddress = Basket.Domain.Entities.ShippingAddress;
using ShippingAddressEvent = EventBus.Messages.Events.ShippingAddress;
using CardEvent = EventBus.Messages.Events.Card;

namespace Basket.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, OrderSubmittedEvent>().ReverseMap();
        CreateMap<ShippingAddress, ShippingAddressEvent>().ReverseMap();
        CreateMap<Card, CardEvent>().ReverseMap();
        CreateMap<ShoppingCartItem, Product>().ReverseMap();
    }
}
using AutoMapper;
using Basket.Domain.Entities;
using EventBus.Messages.Events;

namespace Basket.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, BasketCheckedOutEvent>().ReverseMap();
        CreateMap<ShoppingCartItem, Product>().ReverseMap();
    }
}
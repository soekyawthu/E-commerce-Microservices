using AutoMapper;
using Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;
using Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;
using Basket.Domain.Entities;

namespace Basket.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, CheckoutBasketCommand>().ReverseMap();
        CreateMap<ShoppingCart, UpdateBasketCommand>().ReverseMap();
        CreateMap<ShoppingCartItem, Product>().ReverseMap();
        CreateMap<ShoppingCartItem, CartItem>().ReverseMap();
    }
}
using AutoMapper;
using Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;
using Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;
using Basket.Domain.Entities;
using CardEntity = Basket.Domain.Entities.Card;
using ShippingAddressEntity = Basket.Domain.Entities.ShippingAddress;
using Card = Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket.Card;
using ShippingAddress = Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket.ShippingAddress;

namespace Basket.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, CheckoutBasketCommand>().ReverseMap();
        CreateMap<ShippingAddressEntity, ShippingAddress>().ReverseMap();
        CreateMap<CardEntity, Card>().ReverseMap();
        CreateMap<ShoppingCart, UpdateBasketCommand>().ReverseMap();
        CreateMap<ShoppingCartItem, Product>().ReverseMap();
        CreateMap<ShoppingCartItem, CartItem>().ReverseMap();
    }
}
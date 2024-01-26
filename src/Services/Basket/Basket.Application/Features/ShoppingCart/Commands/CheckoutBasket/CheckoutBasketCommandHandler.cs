using AutoMapper;
using Basket.Application.Contracts.Infrastructure;
using Basket.Application.Contracts.Persistence;
using Basket.Domain.Entities;
using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;

public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, bool>
{
    private readonly ICheckoutService _checkoutService;
    private readonly IBasketRepository _repository;
    private readonly IMapper _mapper;

    public CheckoutBasketCommandHandler(ICheckoutService checkoutService, IBasketRepository repository, IMapper mapper)
    {
        _checkoutService = checkoutService;
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<bool> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        var cart = await _repository.GetBasket(request.UserName);
        if (cart is null)
            return false;
        
        var basket = _mapper.Map<BasketCheckout>(request);
        basket.Items = cart.Items;
        basket.TotalPrice = basket.Items.Sum(x => x.Price * x.Quantity);
        return await _checkoutService.Checkout(basket);
    }
}
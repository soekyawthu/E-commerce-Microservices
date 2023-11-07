using AutoMapper;
using Basket.Application.Contracts.Infrastructure;
using Basket.Application.Contracts.Persistence;
using Basket.Domain.Entities;
using EventBus.Messages.Events;
using MassTransit;

namespace Basket.Infrastructure.Services;

public class CheckoutService : ICheckoutService
{
    private readonly IBasketRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public CheckoutService(IBasketRepository repository, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task<bool> Checkout(BasketCheckout checkout)
    {
        var checkedOutEvent = _mapper.Map<BasketCheckedOutEvent>(checkout);
        await _publishEndpoint.Publish(checkedOutEvent);
        await _repository.DeleteBasket(checkout.UserName);
        return true;
    }
}
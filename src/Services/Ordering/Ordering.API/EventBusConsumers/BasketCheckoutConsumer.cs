using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.EventBusConsumers;

public class BasketCheckoutConsumer : IConsumer<BasketCheckout>
{
    private readonly ILogger<BasketCheckoutConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasketCheckoutConsumer(ILogger<BasketCheckoutConsumer> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }
    
    public async Task Consume(ConsumeContext<BasketCheckout> context)
    {
        var checkoutOrderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);
        await _mediator.Send(checkoutOrderCommand);
        _logger.LogInformation("Checkout Order is successfully completed");
    }
}
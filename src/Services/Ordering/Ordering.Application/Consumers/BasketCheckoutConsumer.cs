using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.Application.Consumers;

public class BasketCheckoutConsumer : IConsumer<OrderSubmittedEvent>
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
    
    public async Task Consume(ConsumeContext<OrderSubmittedEvent> context)
    {
        var checkoutOrderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);
        await _mediator.Send(checkoutOrderCommand);
        _logger.LogInformation("Checkout Order is successfully completed");
    }
}
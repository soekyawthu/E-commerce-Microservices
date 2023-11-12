using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository, IMapper mapper)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order is null)
        {
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        _mapper.Map(request, order, typeof(UpdateOrderCommand), typeof(Order));
        
        await _orderRepository.UpdateAsync(order);
        _logger.LogInformation("Order {OrderId} is successfully updated", order.Id);
    }
}
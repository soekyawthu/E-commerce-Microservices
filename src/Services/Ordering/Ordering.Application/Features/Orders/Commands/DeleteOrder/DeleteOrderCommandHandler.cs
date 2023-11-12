using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly ILogger<DeleteOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);
        if (order is null)
        {                
            throw new NotFoundException(nameof(Order), request.OrderId);
        }

        await _orderRepository.DeleteByIdAsync(order.Id);
        _logger.LogInformation("Order {OrderId} is successfully deleted", order.Id);
    }
}
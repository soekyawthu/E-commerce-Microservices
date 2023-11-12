using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Guid>
{
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public CheckoutOrderCommandHandler(ILogger<CheckoutOrderCommandHandler> logger, 
        IOrderRepository orderRepository,
        IEmailService emailService,
        IMapper mapper)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _emailService = emailService;
        _mapper = mapper;
    }
    
    public async Task<Guid> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request);
        await _orderRepository.AddAsync(order);

        _logger.LogInformation("Order {NewOrderId} is successfully created", order.Id);
        
        //SendMail(order);

        return order.Id;
    }

    private void SendMail(Order order)
    {
        var email = new Email { To = "soesai006@gmail.com", Body = "Order was created.", Subject = "Order was created" };

        try
        {
            _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            _logger.LogError("Order {OrderId} failed due to an error with the mail service: {ExMessage}", 
                order.Id, ex.Message);
        }
    }
}
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.StateMachines.OrderStateMachineActivities;

public class OrderSubmittedActivity : IStateMachineActivity<OrderState, OrderSubmittedEvent>
{
    private readonly IOrderRepository _orderRepository;

    public OrderSubmittedActivity(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public void Probe(ProbeContext context)
    {
        context.CreateScope("publish-order-submitted");
    }

    public void Accept(StateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public async Task Execute(BehaviorContext<OrderState, OrderSubmittedEvent> context, IBehavior<OrderState, OrderSubmittedEvent> next)
    {
        Console.WriteLine($"Shopping Card Id is {context.Message.ShoppingCartId}");
        
        await _orderRepository.AddAsync(new Order
        {
            Id = context.Message.ShoppingCartId,
            UserName = context.Message.UserName,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
            EmailAddress = context.Message.LastName,
            AddressLine = context.Message.AddressLine,
            Country = context.Message.Country,
            State = context.Message.State,
            ZipCode = context.Message.ZipCode,
            CardName = context.Message.CardName,
            CardNumber = context.Message.CardNumber,
            Expiration = context.Message.Expiration,
            Items = context.Message.Items.Select(product => new Ordering.Domain.Entities.Product
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Color = product.Color,
                Price = product.Price,
                Quantity = product.Quantity,
            })
        });
        
        var consumeContext = context.GetPayload<ConsumeContext>();
        var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("exchange:fulfill-order"));
        await sendEndpoint.Send(new FulfillOrder
        {
            ShoppingCartId = context.Message.ShoppingCartId,
            UserName = context.Saga.UserName,
            TotalPrice = context.Saga.TotalPrice,
            CardName = context.Saga.CardName,
            CardNumber = context.Saga.CardNumber,
            Expiration = context.Saga.Expiration,
            PaymentMethod = context.Saga.PaymentMethod,
            Items = context.Saga.Items
        });
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderSubmittedEvent, TException> context, IBehavior<OrderState, OrderSubmittedEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}
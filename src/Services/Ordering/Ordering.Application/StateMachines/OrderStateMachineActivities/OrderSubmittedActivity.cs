using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Card = Ordering.Domain.Entities.Card;
using Product = Ordering.Domain.Entities.Product;
using ShippingAddress = Ordering.Domain.Entities.ShippingAddress;

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
            TotalPrice = context.Message.TotalPrice,
            CreatedDate = context.Message.CreationDate,
            Items = context.Message.Items.Select(product => new Product
            {
                Id = product.ProductId,
                Name = product.ProductName,
                Color = product.Color,
                Price = product.Price,
                Quantity = product.Quantity,
            }),
            ShippingAddress = new ShippingAddress
            {
                FullName = context.Message.ShippingAddress.FullName,
                Email = context.Message.ShippingAddress.Email,
                AddressLine = context.Message.ShippingAddress.AddressLine,
                Country = context.Message.ShippingAddress.Country,
                State = context.Message.ShippingAddress.State,
                City = context.Message.ShippingAddress.City,
                ZipCode = context.Message.ShippingAddress.ZipCode
            },
            PaymentCard = new Card
            {
                Name = context.Message.PaymentCard.Name,
                Number = context.Message.PaymentCard.Number,
                Expiration = context.Message.PaymentCard.Expiration,
                Cvv = context.Message.PaymentCard.Cvv
            }
        });
        
        var consumeContext = context.GetPayload<ConsumeContext>();
        var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("exchange:fulfill-order"));
        await sendEndpoint.Send(new FulfillOrder
        {
            ShoppingCartId = context.Message.ShoppingCartId,
            UserName = context.Saga.UserName,
            TotalPrice = context.Saga.TotalPrice,
            CardName = context.Saga.PaymentCard.Name,
            CardNumber = context.Saga.PaymentCard.Number,
            Expiration = context.Saga.PaymentCard.Expiration,
            Items = context.Saga.Items
        });
        await next.Execute(context).ConfigureAwait(false);
    }

    public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderSubmittedEvent, TException> context, IBehavior<OrderState, OrderSubmittedEvent> next) where TException : Exception
    {
        return next.Faulted(context);
    }
}
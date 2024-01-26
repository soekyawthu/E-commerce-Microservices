using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.StateMachines.OrderStateMachineActivities;

namespace Ordering.Application.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Faulted { get; private set; } = null!;
    public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; } = null!;
    public Event<OrderFulfilmentCompleted> OrderCompleted { get; private set; } = null!;
    public Event<OrderFulfilmentFaulted> OrderFaulted { get; private set; } = null!;
    public Event<CheckOrder>? CheckOrderRequest { get; private set; } = null!;
    
    public OrderStateMachine() 
    {
        Event(() => OrderSubmitted,
            x => x.CorrelateById(context => context.Message.ShoppingCartId));
        
        Event(() => OrderCompleted,
            x => x.CorrelateById(context => context.Message.OrderId));
        
        Event(() => OrderFaulted,
            x => x.CorrelateById(context => context.Message.OrderId));

        Event(() => CheckOrderRequest,
            x =>
            {
                x.CorrelateById(context => context.Message.OrderId);
                x.OnMissingInstance(b =>
                    b.ExecuteAsync(context => context.RespondAsync(new OrderNotFound
                    {
                        OrderId = context.Message.OrderId,
                        Reason = "Order Id may be incorrect!"
                    })));
            });

        InstanceState(x => x.CurrentState);
        
        Initially(
            When(OrderSubmitted)
                .Then(x =>
                {
                    x.Saga.UserName = x.Message.UserName;
                    x.Saga.TotalPrice = x.Message.TotalPrice;
                    x.Saga.PaymentCard = x.Message.PaymentCard;
                    x.Saga.ShippingAddress = x.Message.ShippingAddress;
                    x.Saga.Items = x.Message.Items;
                    x.Saga.SubmitAt = DateTime.Now;
                })
                .Activity(x => x.OfType<OrderSubmittedActivity>())
                .TransitionTo(Submitted));

        During(Submitted,
            Ignore(OrderSubmitted),
            When(OrderCompleted)
                .Then(_ => Console.WriteLine("-> Accepted Order"))
                .TransitionTo(Completed),
            When(OrderFaulted)
                .TransitionTo(Faulted));
        
        DuringAny(
            When(CheckOrderRequest)
                .RespondAsync(x => x.Init<OrderStatus>(new OrderStatus()
                {
                    OrderId = x.Saga.CorrelationId,
                    Status = x.Saga.CurrentState,
                    UserName = x.Saga.UserName,
                    TotalPrice = x.Saga.TotalPrice,
                    OrderDate = x.Saga.SubmitAt,
                    ShippingAddress = x.Saga.ShippingAddress,
                    PaymentCard = x.Saga.PaymentCard,
                    Items = x.Saga.Items
                })));
    }
}
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
            x => x.CorrelateById(context => context.Message.OrderId));

        InstanceState(x => x.CurrentState);
        
        Initially(
            When(OrderSubmitted)
                .Then(x =>
                {
                    x.Saga.UserName = x.Message.UserName;
                    x.Saga.FirstName = x.Message.FirstName;
                    x.Saga.LastName = x.Message.LastName;
                    x.Saga.EmailAddress = x.Message.EmailAddress;
                    x.Saga.AddressLine = x.Message.AddressLine;
                    x.Saga.Country = x.Message.Country;
                    x.Saga.State = x.Message.State;
                    x.Saga.ZipCode = x.Message.ZipCode;
                    x.Saga.CardName = x.Message.CardName;
                    x.Saga.CardNumber = x.Message.CardNumber;
                    x.Saga.Expiration = x.Message.Expiration;
                    x.Saga.PaymentMethod = x.Message.PaymentMethod;
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
                    Status = x.Saga.CurrentState
                })));
    }
}
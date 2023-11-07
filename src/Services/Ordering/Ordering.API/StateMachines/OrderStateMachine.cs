using EventBus.Messages.Events;
using MassTransit;
using Ordering.API.StateMachines.OrderStateMachineActivities;

namespace Ordering.API.StateMachines;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Submitted { get; private set; } = null!;
    public State Accepted { get; private set; } = null!;
    public Event<OrderSubmittedEvent> OrderSubmitted { get; private set; } = null!;
    public Event<OrderAccepted> OrderAccepted { get; private set; } = null!;

    public OrderStateMachine() 
    {
        Event(() => OrderSubmitted,
            x => x.CorrelateById(context => context.Message.ShoppingCartId));
        
        Event(() => OrderAccepted,
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
                .TransitionTo(Submitted));
        
        During(Submitted,
            Ignore(OrderSubmitted),
            When(OrderAccepted)
                .Activity(x => x.OfType<OrderAcceptedActivity>())
                .TransitionTo(Accepted));
    }
}
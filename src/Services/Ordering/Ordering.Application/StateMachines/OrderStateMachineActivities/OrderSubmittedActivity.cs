using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Application.StateMachines.OrderStateMachineActivities;

public class OrderSubmittedActivity : IStateMachineActivity<OrderState, OrderSubmittedEvent>
{
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
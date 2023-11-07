using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.API.StateMachines.OrderStateMachineActivities;

public class OrderAcceptedActivity : IStateMachineActivity<OrderState, OrderAccepted>
{
    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }

    public void Accept(StateMachineVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public async Task Execute(BehaviorContext<OrderState, OrderAccepted> context, IBehavior<OrderState, OrderAccepted> next)
    {
        throw new NotImplementedException();
    }

    public async Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderAccepted, TException> context, IBehavior<OrderState, OrderAccepted> next) where TException : Exception
    {
        throw new NotImplementedException();
    }
}
using MassTransit;

namespace Ordering.Application.StateMachines;

public class OrderStateDefinition : SagaDefinition<OrderState>
{
    public OrderStateDefinition()
    {
        //prefetch count
        ConcurrentMessageLimit = 4;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator,
        IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(x => x.Intervals(1000, 5000, 10000));
        endpointConfigurator.UseMongoDbOutbox(context);
    }
}
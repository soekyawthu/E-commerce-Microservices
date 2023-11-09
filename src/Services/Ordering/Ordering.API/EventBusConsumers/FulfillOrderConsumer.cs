using EventBus.Messages.Commands;
using MassTransit;

namespace Ordering.API.EventBusConsumers;

public class FulfillOrderConsumer : IConsumer<FulfillOrder>
{
    public async Task Consume(ConsumeContext<FulfillOrder> context)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddActivity("DecreaseInventory", new Uri("exchange:decrease-inventory_execute"), context.Message.Items);

        builder.AddActivity("Payment", new Uri("exchange:payment_execute"), new
        {
            context.Message.CardNumber,
            Amount = context.Message.TotalPrice
        });

        builder.AddVariable("OrderId", context.Message.ShoppingCartId);
        
        var routingSlip = builder.Build();
        await context.Execute(routingSlip);
    }
}
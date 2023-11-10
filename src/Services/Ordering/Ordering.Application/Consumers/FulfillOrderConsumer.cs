using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using MassTransit.Courier.Contracts;
using Ordering.Application.CourierActivities;

namespace Ordering.Application.Consumers;

public class FulfillOrderConsumer : IConsumer<FulfillOrder>
{
    public async Task Consume(ConsumeContext<FulfillOrder> context)
    {
        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        builder.AddActivity("ReduceInventory", new Uri("exchange:reduce-inventory_execute"), new ReduceInventoryArgument()
        {
            OrderId = context.Message.ShoppingCartId,
            Items = context.Message.Items
        });

        builder.AddActivity("Payment", new Uri("exchange:payment_execute"), new PaymentArgument()
        {
            OrderId = context.Message.ShoppingCartId,
            PaymentCardNumber = context.Message.CardNumber,
            Amount = context.Message.TotalPrice
        });

        builder.AddVariable("OrderId", context.Message.ShoppingCartId);
        
        await builder.AddSubscription(context.SourceAddress!, 
            RoutingSlipEvents.Faulted | RoutingSlipEvents.Supplemental, 
            RoutingSlipEventContents.None,
            endpoint => endpoint.Send<OrderFulfilmentFaulted>(new
            {
                context.Message.ShoppingCartId,
            }));
        
        await builder.AddSubscription(context.SourceAddress!, 
            RoutingSlipEvents.Completed | RoutingSlipEvents.Supplemental, 
            RoutingSlipEventContents.None,
            endpoint => endpoint.Send<OrderFulfilmentCompleted>(new
            {
                context.Message.ShoppingCartId,
            }));
        
        var routingSlip = builder.Build();
        await context.Execute(routingSlip);
    }
}
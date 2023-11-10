using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Inventory.API.Consumers;

public class ReduceInventoryConsumer : IConsumer<ReduceInventory>
{
    public async Task Consume(ConsumeContext<ReduceInventory> context)
    {
        var items = context.Message.Items.ToList();

        if (!items.Any())
        {
            await context.RespondAsync(new ReduceInventoryRejected
            {
                ReductionId = context.Message.ReductionId,
                Reason = "Item is empty"
            });
        }
        
        await context.RespondAsync(new ReduceInventoryAccepted
        {
            ReductionId = context.Message.ReductionId,
            Message = "Reduced Inventory successfully"
        });
    }
}
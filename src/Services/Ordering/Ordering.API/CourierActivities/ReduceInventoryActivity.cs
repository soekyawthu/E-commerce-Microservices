using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.API.CourierActivities;

public class ReduceInventoryActivity : IActivity<ReduceInventoryArgument, ReduceInventoryLog>
{
    private readonly IRequestClient<ReduceInventory> _requestClient;

    public ReduceInventoryActivity(IRequestClient<ReduceInventory> requestClient)
    {
        _requestClient = requestClient;
    }
    
    public async Task<ExecutionResult> Execute(ExecuteContext<ReduceInventoryArgument> context)
    {
        var items = context.Arguments.Items.ToList();
        if (!items.Any()) throw new InvalidDataException(nameof(items));
        
        var reductionId = NewId.NextGuid();
        await _requestClient.GetResponse<InventoryReducedEvent>(new ReduceInventory
        {
            ReductionId = reductionId,
            Items = items
        });
        
        return context.Completed<ReduceInventoryLog>(new { ReductionId = reductionId });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ReduceInventoryLog> context)
    {
        Console.WriteLine($"Called Compensate Method of DecreaseInventoryActivity - {context.Log.ReductionId}");
        return context.Compensated();
    }
}


public class ReduceInventoryArgument
{
    public Guid OrderId { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}

public class ReduceInventoryLog
{
    public Guid ReductionId { get; set; }
}

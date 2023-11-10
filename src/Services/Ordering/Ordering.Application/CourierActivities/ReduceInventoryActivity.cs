using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Application.CourierActivities;

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
        var (accept, reject) = await _requestClient.GetResponse<ReduceInventoryAccepted, ReduceInventoryRejected>(new ReduceInventory
        {
            ReductionId = reductionId,
            Items = items
        });

        if (!accept.IsCompletedSuccessfully)
        {
            throw new ApplicationException($"Inventory Reduction is failed because {reject.Result.Message.Reason}");
        }
        Console.WriteLine("Complete ReduceInventory Activity");
        return context.Completed<ReduceInventoryLog>(new { ReductionId = reductionId });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<ReduceInventoryLog> context)
    {
        Console.WriteLine($"Called Compensate Method of DecreaseInventoryActivity - {context.Log.ReductionId}");
        await Task.Delay(100);
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

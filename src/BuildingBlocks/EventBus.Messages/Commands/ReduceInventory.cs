using EventBus.Messages.Events;

namespace EventBus.Messages.Commands;

public class ReduceInventory
{
    public Guid ReductionId { get; set; }
    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}
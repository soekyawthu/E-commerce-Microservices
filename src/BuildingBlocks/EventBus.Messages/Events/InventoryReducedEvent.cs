namespace EventBus.Messages.Events;

public class InventoryReducedEvent
{
    public Guid ReductionId { get; set; }
    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}
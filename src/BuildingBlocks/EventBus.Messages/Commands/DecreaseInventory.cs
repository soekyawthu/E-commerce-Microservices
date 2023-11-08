using EventBus.Messages.Events;

namespace EventBus.Messages.Commands;

public class DecreaseInventory
{
    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}
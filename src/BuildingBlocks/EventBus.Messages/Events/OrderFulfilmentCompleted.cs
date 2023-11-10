namespace EventBus.Messages.Events;

public class OrderFulfilmentCompleted
{
    public Guid OrderId { get; set; }
}
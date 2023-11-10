namespace EventBus.Messages.Events;

public class OrderFulfilmentFaulted
{
    public Guid OrderId { get; set; }
}
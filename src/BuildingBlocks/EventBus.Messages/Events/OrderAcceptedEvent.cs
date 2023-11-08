namespace EventBus.Messages.Events;

public class OrderAcceptedEvent
{
    public Guid OrderId { get; set; }
}
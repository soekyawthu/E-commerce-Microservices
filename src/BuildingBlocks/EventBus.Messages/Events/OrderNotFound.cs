namespace EventBus.Messages.Events;

public class OrderNotFound
{
    public Guid OrderId { get; set; }
    public string? Reason { get; set; }
}
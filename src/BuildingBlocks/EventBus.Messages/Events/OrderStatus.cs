namespace EventBus.Messages.Events;

public class OrderStatus
{
    public Guid OrderId { get; set; }
    public string? Status { get; set; }
}
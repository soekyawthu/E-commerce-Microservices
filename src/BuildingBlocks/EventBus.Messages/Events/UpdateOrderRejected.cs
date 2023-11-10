namespace EventBus.Messages.Events;

public class UpdateOrderRejected
{
    public Guid OrderId { get; set; }
    public string? Reason { get; set; }
}
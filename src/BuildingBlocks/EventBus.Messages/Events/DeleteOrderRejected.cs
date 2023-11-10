namespace EventBus.Messages.Events;

public class DeleteOrderRejected
{
    public Guid OrderId { get; set; }
    public string? Reason { get; set; }
}
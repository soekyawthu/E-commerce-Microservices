namespace EventBus.Messages.Events;

public class PaymentRejected
{
    public Guid PaymentId { get; set; }
    public string? Reason { get; set; }
}
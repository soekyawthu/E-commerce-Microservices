namespace EventBus.Messages.Events;

public class PaymentAccepted
{
    public Guid PaymentId { get; set; }
    public string? Message { get; set; }
}
namespace EventBus.Messages.Commands;

public class Pay
{
    public Guid PaymentId { get; set; }
    public required string CardNumber { get; set; }
}
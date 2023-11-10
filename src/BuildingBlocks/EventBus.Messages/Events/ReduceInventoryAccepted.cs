namespace EventBus.Messages.Events;

public class ReduceInventoryAccepted
{
    public Guid ReductionId { get; set; }
    public string? Message { get; set; }
}
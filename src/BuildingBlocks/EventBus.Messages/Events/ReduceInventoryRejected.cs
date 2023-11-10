namespace EventBus.Messages.Events;

public class ReduceInventoryRejected
{
    public Guid ReductionId { get; set; }
    public string? Reason { get; set; }
}
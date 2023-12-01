using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Application.StateMachines;

public class OrderState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string? CurrentState { get; set; }
    public int Version { get; set; }
    
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    public required ShippingAddress ShippingAddress { get; set; }
    public required Card PaymentCard { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
    
    public DateTime SubmitAt { get; set; }
    public DateTime UpdateAt { get; set; }
}
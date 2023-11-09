using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.API.StateMachines;

public class OrderStateMap:
    SagaClassMap<OrderState>
{
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);
        
        entity.Property(x => x.UserName);
        entity.Property(x => x.FirstName);
        entity.Property(x => x.LastName);
        entity.Property(x => x.CardName);
        entity.Property(x => x.CardNumber);
        entity.Property(x => x.Items);
    }
}
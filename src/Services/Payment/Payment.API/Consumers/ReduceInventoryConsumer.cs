using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Payment.API.Consumers;

public class PaymentConsumer : IConsumer<Pay>
{
    public async Task Consume(ConsumeContext<Pay> context)
    {
        var cardNumber = context.Message.CardNumber;
        
        if (cardNumber.StartsWith("TEST"))
        {
            await context.RespondAsync(new PaymentRejected
            {
                PaymentId = context.Message.PaymentId,
                Reason = "Your card number is invalid"
            });
        }
        
        await context.RespondAsync(new PaymentAccepted
        {
            PaymentId = context.Message.PaymentId,
            Message = "Reduced Inventory successfully"
        });
    }
}
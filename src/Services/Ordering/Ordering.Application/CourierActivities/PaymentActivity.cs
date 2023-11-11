using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Application.CourierActivities;

public class PaymentActivity : IActivity<PaymentArgument, PaymentLog>
{
    private readonly IRequestClient<Pay> _requestClient;

    public PaymentActivity(IRequestClient<Pay> requestClient)
    {
        _requestClient = requestClient;
    }
    
    public async Task<ExecutionResult> Execute(ExecuteContext<PaymentArgument> context)
    {
        var cardNumber = context.Arguments.PaymentCardNumber;
        if (string.IsNullOrEmpty(cardNumber))
        {
            Console.WriteLine("\n\n => PaymentCardNumber is Null \n\n");
            throw new InvalidDataException();
        }
        
        var paymentId = Guid.NewGuid();
        var (accept, reject) = await _requestClient.GetResponse<PaymentAccepted, PaymentRejected>(new Pay
        {
            PaymentId = paymentId,
            CardNumber = context.Arguments.PaymentCardNumber!
        });

        if (!accept.IsCompletedSuccessfully)
        {
            throw new ApplicationException($"Payment failed because {reject.Result.Message.Reason}");
        }
        
        Console.WriteLine("Complete Payment Activity");
        await Task.Delay(1000);

        return context.Completed<PaymentLog>(new { AuthorizationCode = "7777" });
    }

    public async Task<CompensationResult> Compensate(CompensateContext<PaymentLog> context)
    {
        Console.WriteLine($"Call Compensate Method of PaymentActivity - {context.Log.AuthorizationCode}");
        await Task.Delay(1000);
        return context.Compensated();
    }
}

public class PaymentArgument
{
    public Guid OrderId { get; set; }
    public string? PaymentCardNumber { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentLog
{
    public string? AuthorizationCode { get; set; }
}
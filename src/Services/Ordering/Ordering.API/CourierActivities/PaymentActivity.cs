using MassTransit;

namespace Ordering.API.CourierActivities;

public class PaymentActivity : IActivity<PaymentArgument, PaymentLog>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<PaymentArgument> context)
    {
        var cardNumber = context.Arguments.PaymentCardNumber;
        if (string.IsNullOrEmpty(cardNumber))
        {
            Console.WriteLine("\n\n => PaymentCardNumber is Null \n\n");
            throw new InvalidDataException();
        }
        if (cardNumber.StartsWith("1234"))
        {
            throw new InvalidOperationException();
        }
        
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
    public Guid? OrderId { get; set; }
    public string? PaymentCardNumber { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentLog
{
    public string? AuthorizationCode { get; set; }
}
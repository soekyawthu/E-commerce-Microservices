using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace Shopping.Aggregator;

public class HttpPolicy
{
    internal static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (result, timespan, retryCount, context) =>
                {
                    Log.Error("Retry {RetryCount} of {PolicyKey} at {OperationKey} & {Timespan}, due to: {Result}",
                        retryCount, context.PolicyKey, context.OperationKey, timespan, result.Result);
                });
    }

    internal static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
    }
}
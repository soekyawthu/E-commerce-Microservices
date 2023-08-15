using System.Text.Json;

namespace Shopping.Aggregator.Extensions;

public static class HttpClientExtension
{
    public static async Task<T?> ReadContentAs<T>(this HttpResponseMessage responseMessage)
    {
        if (!responseMessage.IsSuccessStatusCode)
            throw new ApplicationException($"Failed Calling API : {responseMessage.ReasonPhrase}");

        var dataAsString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
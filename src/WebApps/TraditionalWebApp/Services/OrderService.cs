using TraditionalWebApp.Extensions;
using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username)
    {
        var response = await _httpClient.GetAsync($"Order/{username}");
        return await response.ReadContentAs<List<OrderResponseModel>>() ?? new List<OrderResponseModel>();
    }
}

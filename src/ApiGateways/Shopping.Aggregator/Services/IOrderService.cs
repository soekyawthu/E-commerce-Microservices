using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface IOrderService
{
    public Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username);
}
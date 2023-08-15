using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public interface IOrderService
{
    public Task<IEnumerable<OrderResponseModel>> GetOrdersByUsername(string username);
}
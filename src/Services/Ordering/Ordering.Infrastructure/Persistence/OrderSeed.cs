using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence;

public class OrderSeed
{
    public static async Task SeedAsync(ILogger<OrderSeed>? logger, OrderDbContext context)
    {
        if (!context.Orders.Any())
        {
            await context.Orders.AddRangeAsync(GetPreconfiguredOrders());
            await context.SaveChangesAsync();
            logger?.LogInformation("Seed database associated with context {DbContextName}", nameof(OrderDbContext));
        }
    }
    
    private static IEnumerable<Order> GetPreconfiguredOrders()
    {
        return new List<Order>
        {
            new()
            {
                UserName = "skt", 
                FirstName = "Soe Kyaw", 
                LastName = "Thu", 
                EmailAddress = "soekyawthu.dev@gmail.com", 
                AddressLine = "Yangon", 
                Country = "Myanmar", 
                TotalPrice = 350
            }
        };
    }
}
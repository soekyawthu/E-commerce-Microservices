using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence.Mongo;

namespace Ordering.Infrastructure.Persistence;

public class OrderSeed
{
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
                TotalPrice = 350,
                State = string.Empty,
                ZipCode = string.Empty,
                CardName = string.Empty,
                CardNumber = string.Empty,
                Expiration = string.Empty,
            }
        };
    }
}
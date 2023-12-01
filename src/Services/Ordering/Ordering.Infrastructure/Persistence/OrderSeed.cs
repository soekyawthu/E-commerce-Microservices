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
                ShippingAddress = new ShippingAddress
                    {
                        FullName = "Soe Kyaw Thu",
                        Email = "soekyawthu.dev@gmail.com",
                        AddressLine = "Pale",
                        Country = "Myanmar",
                        State = "Sagaing",
                        City = "Pale",
                        ZipCode = 111
                    },
                PaymentCard = new Card
                {
                    Name = "Soe Kyaw Thu",
                    Number = "422 422 422 422",
                    Expiration = DateTime.Now,
                    Cvv = "abc"
                },
                TotalPrice = 350
            }
        };
    }
}
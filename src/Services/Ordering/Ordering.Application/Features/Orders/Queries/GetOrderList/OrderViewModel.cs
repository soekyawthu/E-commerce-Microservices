using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class OrderViewModel
{
    public Guid Id { get; set; }
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    public required ShippingAddress ShippingAddress { get; set; }
    public required Card PaymentCard { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}

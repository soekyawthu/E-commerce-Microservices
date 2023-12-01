using Ordering.Domain.Common;

namespace Ordering.Domain.Entities;

public class Order : EntityBase
{
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    public required ShippingAddress ShippingAddress { get; set; }
    public required Card PaymentCard { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}

public class ShippingAddress
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string AddressLine { get; set; }
    public required string Country { get; set; }
    public required string State { get; set; }
    public required string City { get; set; }
    public required int ZipCode { get; set; }
}

public class Card
{
    public required string Name { get; set; }
    public required string Number { get; set; }
    public required DateTime Expiration { get; set; }
    public required string Cvv { get; set; }
}
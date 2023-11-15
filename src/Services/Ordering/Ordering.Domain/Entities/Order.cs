using Ordering.Domain.Common;

namespace Ordering.Domain.Entities;

public class Order : EntityBase
{
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    // BillingAddress
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public required string AddressLine { get; set; }
    public required string Country { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }

    // Payment
    public required string CardName { get; set; }
    public required string CardNumber { get; set; }
    public required string Expiration { get; set; }
    public int PaymentMethod { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}
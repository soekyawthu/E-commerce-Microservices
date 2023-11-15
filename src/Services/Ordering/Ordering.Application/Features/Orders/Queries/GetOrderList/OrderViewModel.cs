using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public class OrderViewModel
{
    public Guid Id { get; set; }
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
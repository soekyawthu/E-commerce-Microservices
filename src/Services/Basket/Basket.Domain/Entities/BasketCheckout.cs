namespace Basket.Domain.Entities;

public class BasketCheckout
{
    public Guid ShoppingCartId { get; set; }
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

    public IEnumerable<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
}
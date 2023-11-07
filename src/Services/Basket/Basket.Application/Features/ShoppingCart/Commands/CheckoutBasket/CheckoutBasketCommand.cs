using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;

public class CheckoutBasketCommand : IRequest<bool>
{
    public Guid ShoppingCartId { get; set; }
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }

    // BillingAddress
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? EmailAddress { get; set; }
    public string? AddressLine { get; set; }
    public string? Country { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }

    // Payment
    public string? CardName { get; set; }
    public string? CardNumber { get; set; }
    public string? Expiration { get; set; }
    public int PaymentMethod { get; set; }

    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}

public class Product
{
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
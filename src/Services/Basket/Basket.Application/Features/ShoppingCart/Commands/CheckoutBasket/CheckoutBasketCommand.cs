using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;

public class CheckoutBasketCommand : IRequest<bool>
{
    public Guid ShoppingCartId { get; set; }
    public required string UserName { get; set; }
    public decimal TotalPrice { get; set; }
    
    public required ShippingAddress ShippingAddress { get; set; }
    public required Card PaymentCard { get; set; }
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

public class Product
{
    public string? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
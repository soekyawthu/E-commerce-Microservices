namespace Basket.Domain.Entities;

public class ShoppingCartItem
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Color { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
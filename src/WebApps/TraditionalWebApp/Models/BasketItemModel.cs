namespace TraditionalWebApp.Models;

public class BasketItemModel
{
    public required int Quantity { get; set; }
    public required string Color { get; set; }
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required decimal Price { get; set; }
}
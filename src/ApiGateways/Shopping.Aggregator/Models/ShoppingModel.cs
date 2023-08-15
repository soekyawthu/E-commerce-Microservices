namespace Shopping.Aggregator.Models;

public class ShoppingModel
{
    public required string Username { get; set; }
    public required BasketModel BasketWithProducts { get; set; }
    public IEnumerable<OrderResponseModel> Orders { get; set; } = new List<OrderResponseModel>();
}
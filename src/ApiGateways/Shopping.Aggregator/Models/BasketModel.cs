namespace Shopping.Aggregator.Models;

public class BasketModel
{
    public required string Username { get; set; }
    public IEnumerable<BasketItemExtendedModel> Items { get; set; } = new List<BasketItemExtendedModel>();
    public decimal TotalPrice { get; set; }
}
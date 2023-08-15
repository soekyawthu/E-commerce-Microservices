namespace TraditionalWebApp.Models;

public class BasketModel
{
    public required string Username { get; set; }
    public List<BasketItemModel> Items { get; set; } = new List<BasketItemModel>();
    public decimal TotalPrice { get; set; }
}
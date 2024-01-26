namespace EventBus.Messages.Events;

public class OrderStatus
{
    public Guid OrderId { get; set; }
    public string? Status { get; set; }
    
    public string? UserName { get; set; }
    public decimal TotalPrice { get; set; }
    public ShippingAddress? ShippingAddress { get; set; }
    public Card? PaymentCard { get; set; }
    public DateTime OrderDate { get; set; }
    public IEnumerable<Product> Items { get; set; } = new List<Product>();
}
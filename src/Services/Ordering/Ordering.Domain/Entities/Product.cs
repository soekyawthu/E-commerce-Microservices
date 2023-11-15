using Ordering.Domain.Common;

namespace Ordering.Domain.Entities;

public class Product : EntityBase
{
    public string? Name { get; set; }
    public required string Color { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
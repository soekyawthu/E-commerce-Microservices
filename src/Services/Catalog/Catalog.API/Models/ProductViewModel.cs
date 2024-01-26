using System.Text.Json.Serialization;

namespace Catalog.API.Models;

public class ProductViewModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Summary { get; set; }
    public required string Description { get; set; }
    
    [JsonIgnore]
    public required IFormFile ImageFile { get; set; }
    public string? Image { get; set; }
    
    public decimal Price { get; set; }
}
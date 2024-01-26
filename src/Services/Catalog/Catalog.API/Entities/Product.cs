using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities;

public class Product
{
    [BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    
    [BsonElement("Name")]
    public required string Name { get; set; }
    public required string Category { get; set; }
    public required string Summary { get; set; }
    public required string Description { get; set; }
    
    [BsonIgnore]
    [JsonIgnore]
    public IFormFile? ImageFile { get; set; }
    public string? Image { get; set; }
    public decimal Price { get; set; }
}
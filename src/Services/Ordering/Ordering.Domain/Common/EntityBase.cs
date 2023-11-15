using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ordering.Domain.Common;

public class EntityBase
{
    public Guid Id { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
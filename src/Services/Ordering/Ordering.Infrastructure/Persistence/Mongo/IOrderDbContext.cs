using MongoDB.Driver;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Mongo;

public interface IOrderDbContext
{
    IMongoCollection<Order> Orders { get; }
}
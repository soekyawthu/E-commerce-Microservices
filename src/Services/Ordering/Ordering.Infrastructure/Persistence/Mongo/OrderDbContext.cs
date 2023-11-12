using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.Mongo;

public class OrderDbContext: IOrderDbContext
{
    public OrderDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Orders = database.GetCollection<Order>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
    }
    
    public IMongoCollection<Order> Orders { get; }
}
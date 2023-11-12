using MongoDB.Driver;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence.Mongo;

namespace Ordering.Infrastructure.Repositories;

public interface IOrderMongoRepository
{
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(Guid id);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteByIdAsync(Guid orderId);
}

public class OrderMongoRepository : IOrderRepository
{
    private readonly IOrderDbContext _dbContext;

    public OrderMongoRepository(IOrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyList<Order>> GetAllAsync()
    {
        return await _dbContext.Orders
            .Find(o => true)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Orders
            .Find(o => o.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Order order)
    {
        await _dbContext.Orders
            .InsertOneAsync(order);
    }

    public async Task UpdateAsync(Order order)
    {
        await _dbContext.Orders
            .ReplaceOneAsync(o => o.Id == order.Id, order);
    }

    public async Task DeleteByIdAsync(Guid orderId)
    {
        await _dbContext.Orders
            .DeleteOneAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        return await _dbContext.Orders
            .Find(o => o.UserName == userName)
            .ToListAsync();
    }
}
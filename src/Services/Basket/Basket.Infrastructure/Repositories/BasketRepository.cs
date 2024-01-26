using Basket.Application.Contracts.Persistence;
using Basket.Domain.Entities;
using MassTransit.MongoDbIntegration;
using MongoDB.Driver;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly MongoDbContext _dbContext;
    private readonly IMongoCollection<ShoppingCart> _shoppingCarts;

    public BasketRepository(MongoDbContext dbContext, IMongoCollection<ShoppingCart> shoppingCarts)
    {
        _dbContext = dbContext;
        _shoppingCarts = shoppingCarts;
    }
    
    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var filter = Builders<ShoppingCart>.Filter.Eq(x => x.UserName, userName);
        return await _shoppingCarts.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<ShoppingCart?> CreateOrUpdateBasket(ShoppingCart basket)
    {
        var filter = Builders<ShoppingCart>.Filter.Eq(x => x.UserName, basket.UserName);
        var shoppingCart = await _shoppingCarts.Find(filter).FirstOrDefaultAsync();
        
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await _dbContext.BeginTransaction(cancellationTokenSource.Token);

        if (shoppingCart is not null)
        {
            await _shoppingCarts.ReplaceOneAsync(_dbContext.Session, filter, basket, cancellationToken: cancellationTokenSource.Token);
        }
        else
        {
            await _shoppingCarts.InsertOneAsync(_dbContext.Session, basket,
                cancellationToken: cancellationTokenSource.Token);
        }
        
        try
        {
            await _dbContext.CommitTransaction(cancellationTokenSource.Token);
        }
        catch (MongoCommandException exception) when (exception.CodeName == "DuplicateKey")
        {
            throw new Exception("Duplicate registration", exception);
        }

        return basket;
    }

    public async Task DeleteBasket(string userName)
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        await _dbContext.BeginTransaction(cancellationTokenSource.Token);
        var filter = Builders<ShoppingCart>.Filter.Eq(x => x.UserName, userName);
        await _shoppingCarts.DeleteOneAsync(_dbContext.Session, filter, cancellationToken: cancellationTokenSource.Token);
        
        try
        {
            await _dbContext.CommitTransaction(cancellationTokenSource.Token);
        }
        catch (MongoCommandException exception) when (exception.CodeName == "DuplicateKey")
        {
            throw new Exception("Duplicate registration", exception);
        }
    }
}
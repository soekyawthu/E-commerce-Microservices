using Basket.Domain.Entities;

namespace Basket.Application.Contracts.Persistence;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName);
    Task<ShoppingCart?> CreateOrUpdateBasket(ShoppingCart basket);
    Task DeleteBasket(string userName);
}
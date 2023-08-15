using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public interface IBasketService
{
    public Task<BasketModel?> GetBasket(string username);

    public Task<BasketModel?> UpdateBasket(BasketModel basket);

    public Task CheckoutBasket(CheckoutModel checkout);
}
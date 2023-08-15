using TraditionalWebApp.Extensions;
using TraditionalWebApp.Models;

namespace TraditionalWebApp.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;

    public BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<BasketModel?> GetBasket(string username)
    {
        var response = await _httpClient.GetAsync($"basket/{username}");
        return await response.ReadContentAs<BasketModel>();
    }

    public async Task<BasketModel?> UpdateBasket(BasketModel basket)
    {
        var response = await _httpClient.PostAsJson("basket", basket);
        return await response.ReadContentAs<BasketModel>();
    }

    public async Task CheckoutBasket(CheckoutModel checkout)
    {
        var response = await _httpClient.PostAsJson("basket/checkout", checkout);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Something went wrong when calling api.");
        }
    }
}
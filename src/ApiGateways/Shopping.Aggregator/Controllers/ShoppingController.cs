using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalogService;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
    {
        _catalogService = catalogService;
        _basketService = basketService;
        _orderService = orderService;
    }

    [HttpGet("{username}", Name = "GetShopping")]
    public async Task<IActionResult> GetShopping(string username)
    {
        var basket = await _basketService.GetBasket(username);

        foreach (var item in basket?.Items!)
        {
            var catalog = await _catalogService.GetCatalog(item.ProductId);
            item.Category = catalog?.Category;
            item.Summary = catalog?.Summary;
            item.Description = catalog?.Description;
            item.ImageFile = catalog?.ImageFile;
        }

        var orders = await _orderService.GetOrdersByUsername(username);

        var shopping = new ShoppingModel
        {
            Username = username,
            BasketWithProducts = basket,
            Orders = orders
        };

        return Ok(shopping);
    }
}
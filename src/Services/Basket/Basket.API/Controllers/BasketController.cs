using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketRepository _repository;
    private readonly DiscountService _discountService;

    public BasketController(ILogger<BasketController> logger, IBasketRepository repository, DiscountService discountService)
    {
        _logger = logger;
        _repository = repository;
        _discountService = discountService;
    }
    
    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _repository.GetBasket(userName);
        return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        foreach (var shoppingCartItem in basket.Items)
        {
            var coupon = await _discountService.GetDiscount(shoppingCartItem.ProductName!);
            _logger.LogInformation("Coupon Product - {ProductName}, Discount Amount - {Amount}", coupon.ProductName, coupon.Amount);
            shoppingCartItem.Price -= coupon.Amount;
        }

        var result = await _repository.UpdateBasket(basket);
        return Ok(result);
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

}
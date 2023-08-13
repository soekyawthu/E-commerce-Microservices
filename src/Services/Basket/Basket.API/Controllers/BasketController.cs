using System.Net;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketRepository _repository;
    private readonly DiscountService _discountService;
    private readonly IPublishEndpoint _publishEndpoint;

    public BasketController(ILogger<BasketController> logger, 
        IBasketRepository repository, 
        DiscountService discountService,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _repository = repository;
        _discountService = discountService;
        _publishEndpoint = publishEndpoint;
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

    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<ShoppingCart>> Checkout([FromBody] BasketCheckout checkout)
    {
        var basket = await _repository.GetBasket(checkout.UserName);

        if (basket is null)
            return BadRequest();

        checkout.TotalPrice = basket.TotalPrice;
        
        await _publishEndpoint.Publish(checkout);

        await _repository.DeleteBasket(checkout.UserName);
        
        _logger.LogInformation("Completed Checkout Operation");
        
        return Ok();
    }
}
using System.Net;
using Basket.API.GrpcServices;
using Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;
using Basket.Application.Features.ShoppingCart.Commands.DeleteBasket;
using Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;
using Basket.Application.Features.ShoppingCart.Queries.GetBasket;
using Basket.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IMediator _mediator;
    private readonly DiscountService _discountService;

    public BasketController(ILogger<BasketController> logger, 
        IMediator mediator,
        DiscountService discountService)
    {
        _logger = logger;
        _mediator = mediator;
        _discountService = discountService;
    }
    
    [HttpGet("{userName}", Name = "GetBasket")]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _mediator.Send(new GetBasketQuery { Username = userName });
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] UpdateBasketCommand command)
    {
        /*foreach (var shoppingCartItem in basket.Items)
        {
            var coupon = await _discountService.GetDiscount(shoppingCartItem.ProductName);
            _logger.LogInformation("Coupon Product - {ProductName}, Discount Amount - {Amount}", coupon.ProductName, coupon.Amount);
            shoppingCartItem.Price -= coupon.Amount;
        }*/

        var result = await _mediator.Send(command);

        return Accepted();
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _mediator.Send(new DeleteBasketCommand { Username = userName });
        return Ok();
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> Checkout([FromBody] CheckoutBasketCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok();
    }
}
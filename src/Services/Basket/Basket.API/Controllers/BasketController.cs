using Basket.API.GrpcServices;
using Basket.Application.Features.ShoppingCart.Commands.CheckoutBasket;
using Basket.Application.Features.ShoppingCart.Commands.DeleteBasket;
using Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;
using Basket.Application.Features.ShoppingCart.Queries.GetBasket;
using Basket.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        try
        {
            var basket = await _mediator.Send(new GetBasketQuery { Username = userName });
            return Ok(basket);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new { e.Message });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to get basket";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = errorMessage });
        }
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

        try
        {
            await _mediator.Send(command);
            return Accepted();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new { e.Message });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to add or update basket";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = errorMessage });
        }
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        try
        {
            await _mediator.Send(new DeleteBasketCommand { Username = userName });
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new { e.Message });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to delete basket";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = errorMessage });
        }
    }

    [Route("[action]")]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ShoppingCart>> Checkout([FromBody] CheckoutBasketCommand command)
    {
        try
        {
            await _mediator.Send(command);
            return Accepted();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning(e, "Client made a bad request");
            return BadRequest(new { e.Message });
        }
        catch (Exception e)
        {
            const string errorMessage = "Error while processing request to checkout basket";
            _logger.LogError(e, errorMessage);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = errorMessage });
        }
    }
}
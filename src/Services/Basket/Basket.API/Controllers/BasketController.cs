using System.Net;
using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketRepository _repository;

    public BasketController(ILogger<BasketController> logger, IBasketRepository repository)
    {
        _logger = logger;
        _repository = repository;
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
        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

}
using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IRequestClient<DeleteOrder> _deleteOrderRequestClient;
    private readonly IRequestClient<UpdateOrder> _updateOrderRequestClient;
    private readonly IRequestClient<CheckOrder> _checkOrderRequestClient;

    public OrderController(
        ILogger<OrderController> logger, 
        IRequestClient<DeleteOrder> deleteOrderRequestClient,
        IRequestClient<UpdateOrder> updateOrderRequestClient,
        IRequestClient<CheckOrder> checkOrderRequestClient)
    {
        _logger = logger;
        _deleteOrderRequestClient = deleteOrderRequestClient;
        _updateOrderRequestClient = updateOrderRequestClient;
        _checkOrderRequestClient = checkOrderRequestClient;
    }
    
    [HttpGet("{userName}", Name = "GetOrder")]
    public async Task<ActionResult> GetOrdersByUserName(string userName)
    {
        var (accepted, rejected) = await _checkOrderRequestClient
            .GetResponse<OrderStatus, OrderNotFound>(new CheckOrder{ Username = userName });
        
        if (accepted.IsCompletedSuccessfully)
            return Ok(accepted.Result.Message);
        
        return BadRequest(rejected.Result.Message);
    }
    
    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrder order)
    {
        var (accepted, rejected) = await _updateOrderRequestClient
            .GetResponse<UpdateOrderAccepted, UpdateOrderRejected>(order);
        
        if (accepted.IsCompletedSuccessfully)
            return NoContent();
        
        return BadRequest(rejected.Result.Message);
    }

    [HttpDelete("{id:guid}", Name = "DeleteOrder")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(Guid id)
    {
        var (accepted, rejected) = await _deleteOrderRequestClient
            .GetResponse<DeleteOrderAccepted, DeleteOrderRejected>(new DeleteOrder { OrderId = id });

        if (accepted.IsCompletedSuccessfully)
            return NoContent();
        
        return BadRequest(rejected.Result.Message);
    }
}
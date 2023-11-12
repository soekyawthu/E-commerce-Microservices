using EventBus.Messages.Commands;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IRequestClient<UpdateOrder> _updateOrderRequestClient;
    private readonly IRequestClient<CheckOrder> _checkOrderRequestClient;
    private readonly IMediator _mediator;

    public OrderController(
        ILogger<OrderController> logger, 
        IRequestClient<UpdateOrder> updateOrderRequestClient,
        IRequestClient<CheckOrder> checkOrderRequestClient,
        IMediator mediator)
    {
        _logger = logger;
        _updateOrderRequestClient = updateOrderRequestClient;
        _checkOrderRequestClient = checkOrderRequestClient;
        _mediator = mediator;
    }
    
    [HttpGet("{userName}", Name = "GetOrder")]
    public async Task<ActionResult> GetOrdersByUserName(string userName)
    {
        var result = await _mediator.Send(new GetOrderListQuery(userName));
        return Ok(result);
    }
    
    [HttpGet("{orderId}", Name = "GetOrder")]
    [Route("[action]")]
    public async Task<ActionResult> CheckOrder(Guid orderId)
    {
        var (accepted, rejected) = await _checkOrderRequestClient
            .GetResponse<OrderStatus, OrderNotFound>(new CheckOrder{ OrderId = orderId });
        
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
        await _mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }
}
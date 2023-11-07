using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.DeleteBasket;

public class DeleteBasketCommand : IRequest<bool>
{
    public required string Username { get; set; }
}
using MediatR;

namespace Basket.Application.Features.ShoppingCart.Queries.GetBasket;

public class GetBasketQuery : IRequest<Domain.Entities.ShoppingCart>
{
    public required string Username { get; set; }
}
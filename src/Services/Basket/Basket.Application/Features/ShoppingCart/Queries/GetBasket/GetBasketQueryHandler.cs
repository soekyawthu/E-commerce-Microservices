using Basket.Application.Contracts.Persistence;
using MediatR;

namespace Basket.Application.Features.ShoppingCart.Queries.GetBasket;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Domain.Entities.ShoppingCart>
{
    private readonly IBasketRepository _repository;

    public GetBasketQueryHandler(IBasketRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Domain.Entities.ShoppingCart> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _repository.GetBasket(request.Username);
        return basket ?? new Domain.Entities.ShoppingCart(request.Username);
    }
}
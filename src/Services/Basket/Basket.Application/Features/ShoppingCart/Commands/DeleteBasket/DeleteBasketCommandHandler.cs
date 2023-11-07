using Basket.Application.Contracts.Persistence;
using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.DeleteBasket;

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, bool>
{
    private readonly IBasketRepository _repository;

    public DeleteBasketCommandHandler(IBasketRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<bool> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteBasket(request.Username);
        return true;
    }
}
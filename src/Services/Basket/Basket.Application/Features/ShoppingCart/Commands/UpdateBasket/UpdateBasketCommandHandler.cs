using AutoMapper;
using Basket.Application.Contracts.Persistence;
using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;

public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, bool>
{
    private readonly IBasketRepository _repository;
    private readonly IMapper _mapper;

    public UpdateBasketCommandHandler(IBasketRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<bool> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
    {
        request.ShoppingCartId = request.ShoppingCartId == Guid.Empty ? Guid.NewGuid() : request.ShoppingCartId;
        var shoppingCart = _mapper.Map<Domain.Entities.ShoppingCart>(request);
        var cart = await _repository.CreateOrUpdateBasket(shoppingCart);

        return cart is not null;
    }
}

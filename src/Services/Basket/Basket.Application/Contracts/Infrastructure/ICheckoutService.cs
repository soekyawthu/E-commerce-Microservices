using Basket.Domain.Entities;

namespace Basket.Application.Contracts.Infrastructure;

public interface ICheckoutService
{
    Task<bool> Checkout(BasketCheckout checkout);
}
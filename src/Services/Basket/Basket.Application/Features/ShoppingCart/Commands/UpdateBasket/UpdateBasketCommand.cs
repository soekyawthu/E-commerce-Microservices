using MediatR;

namespace Basket.Application.Features.ShoppingCart.Commands.UpdateBasket;

public class UpdateBasketCommand : IRequest<bool>
{
    public Guid ShoppingCartId { get; set; }
    public string? UserName { get; set; }
    public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();

    public decimal TotalPrice => Items.Sum(item => item.Price * item.Quantity);
}

public class CartItem
{
    public required string ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string Color { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
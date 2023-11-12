using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList;

public record GetOrderListQuery(string UserName) : IRequest<List<OrderViewModel>>;
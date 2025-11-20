using MediatR;
using System.Collections.Generic;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Orders.Queries.GetUserOrders;

public record GetUserOrdersQuery : IRequest<Result<List<OrderDto>>>;

using MediatR;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Carts.Queries.GetCart;

public record GetCartQuery : IRequest<Result<CartDto>>;

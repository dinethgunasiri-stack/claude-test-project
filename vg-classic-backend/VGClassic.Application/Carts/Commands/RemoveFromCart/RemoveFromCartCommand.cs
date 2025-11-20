using MediatR;
using VGClassic.Application.Carts.Queries.GetCart;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Carts.Commands.RemoveFromCart;

public record RemoveFromCartCommand : IRequest<Result<CartDto>>
{
    public int ItemId { get; init; }
}

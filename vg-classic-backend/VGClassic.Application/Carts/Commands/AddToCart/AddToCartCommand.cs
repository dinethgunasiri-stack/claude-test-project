using MediatR;
using VGClassic.Application.Carts.Queries.GetCart;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Carts.Commands.AddToCart;

public record AddToCartCommand : IRequest<Result<CartDto>>
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public int? VariantId { get; init; }
}

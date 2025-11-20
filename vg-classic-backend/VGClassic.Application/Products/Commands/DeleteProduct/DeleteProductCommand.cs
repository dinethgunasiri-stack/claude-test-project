using MediatR;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand : IRequest<Result>
{
    public int Id { get; init; }
}

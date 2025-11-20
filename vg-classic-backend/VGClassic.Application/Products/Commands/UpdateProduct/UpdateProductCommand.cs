using MediatR;
using VGClassic.Application.Common.Models;
using VGClassic.Application.Products.Commands.CreateProduct;

namespace VGClassic.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<Result>
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string DetailedDescription { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal? CompareAtPrice { get; init; }
    public int CategoryId { get; init; }
    public string Brand { get; init; } = "VG Classic";
    public int StockQuantity { get; init; }
    public bool IsFeatured { get; init; }
    public bool IsActive { get; init; } = true;
}

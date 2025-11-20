using MediatR;
using VGClassic.Application.Common.Models;
using VGClassic.Application.Products.Queries.GetProducts;

namespace VGClassic.Application.Products.Queries.GetProductById;

public record GetProductByIdQuery : IRequest<Result<ProductDetailDto>>
{
    public int Id { get; init; }
}

public class ProductDetailDto : ProductDto
{
    public string DetailedDescription { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public int ViewCount { get; set; }
}

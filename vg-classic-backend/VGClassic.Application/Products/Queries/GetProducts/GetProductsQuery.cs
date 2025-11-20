using MediatR;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<Result<PaginatedList<ProductDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 12;
    public int? CategoryId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? SearchTerm { get; init; }
    public string SortBy { get; init; } = "name";
    public string SortOrder { get; init; } = "asc";
    public bool? IsFeatured { get; init; }
}

using System.Collections.Generic;
using MediatR;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<Result<int>>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string DetailedDescription { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public decimal? CompareAtPrice { get; init; }
    public int CategoryId { get; init; }
    public string Brand { get; init; } = "VG Classic";
    public string SKU { get; init; } = string.Empty;
    public int StockQuantity { get; init; }
    public bool IsFeatured { get; init; }
    public List<string> ImageUrls { get; init; } = new();
    public List<CreateProductVariantDto> Variants { get; init; } = new();
}

public class CreateProductVariantDto
{
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public int StockQuantity { get; set; }
}

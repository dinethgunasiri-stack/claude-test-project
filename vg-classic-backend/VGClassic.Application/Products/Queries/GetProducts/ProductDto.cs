using System.Collections.Generic;

namespace VGClassic.Application.Products.Queries.GetProducts;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public List<string> Images { get; set; } = new();
    public List<ProductVariantDto> Variants { get; set; } = new();
    public bool IsFeatured { get; set; }
}

public class ProductVariantDto
{
    public int Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public bool IsInStock { get; set; }
    public decimal AdditionalPrice { get; set; }
}

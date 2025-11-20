using VGClassic.Domain.Common;

namespace VGClassic.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
}

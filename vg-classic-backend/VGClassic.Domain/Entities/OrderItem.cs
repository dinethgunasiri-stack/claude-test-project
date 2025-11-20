using VGClassic.Domain.Common;

namespace VGClassic.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int? VariantId { get; set; }
    public ProductVariant? Variant { get; set; }

    // Snapshot data (prices at time of order)
    public string ProductName { get; set; } = string.Empty;
    public string? VariantSize { get; set; }
    public string? VariantColor { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}

using VGClassic.Domain.Common;

namespace VGClassic.Domain.Entities;

public class CartItem : BaseEntity
{
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int? VariantId { get; set; }
    public ProductVariant? Variant { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

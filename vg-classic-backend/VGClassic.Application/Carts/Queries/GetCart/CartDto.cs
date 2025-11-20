using System.Collections.Generic;
using System.Linq;

namespace VGClassic.Application.Carts.Queries.GetCart;

public class CartDto
{
    public int Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    public int ItemCount => Items.Sum(i => i.Quantity);
}

public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Subtotal => Quantity * Price;
    public string? VariantSize { get; set; }
    public string? VariantColor { get; set; }
}

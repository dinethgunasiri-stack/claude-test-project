using System;
using VGClassic.Domain.Enums;

namespace VGClassic.Application.Orders.Queries.GetUserOrders;

public class OrderDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public int ItemCount { get; set; }
}

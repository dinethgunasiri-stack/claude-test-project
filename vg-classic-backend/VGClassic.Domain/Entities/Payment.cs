using System;
using VGClassic.Domain.Common;
using VGClassic.Domain.Enums;

namespace VGClassic.Domain.Entities;

public class Payment : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public decimal Amount { get; set; }
    public string? TransactionId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentGateway { get; set; }
    public string? PaymentDetails { get; set; }
}

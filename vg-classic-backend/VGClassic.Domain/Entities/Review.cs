using VGClassic.Domain.Common;

namespace VGClassic.Domain.Entities;

public class Review : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public bool IsVerifiedPurchase { get; set; }
    public bool IsApproved { get; set; }
    public int HelpfulCount { get; set; }
}

using System;
using System.Collections.Generic;
using VGClassic.Domain.Common;

namespace VGClassic.Domain.Entities;

public class Cart : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    public DateTime? LastActivityDate { get; set; }
}

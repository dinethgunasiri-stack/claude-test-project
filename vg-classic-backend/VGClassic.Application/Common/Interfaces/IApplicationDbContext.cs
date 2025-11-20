using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VGClassic.Domain.Entities;

namespace VGClassic.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<ProductVariant> ProductVariants { get; }
    DbSet<Category> Categories { get; }
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Address> Addresses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

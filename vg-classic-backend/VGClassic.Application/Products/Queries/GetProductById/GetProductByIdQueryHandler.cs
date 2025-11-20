using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Exceptions;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;
using VGClassic.Application.Products.Queries.GetProducts;

namespace VGClassic.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDetailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductDetailDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == request.Id && p.IsActive, cancellationToken);

        if (product == null)
        {
            return Result<ProductDetailDto>.Failure("Product not found");
        }

        // Increment view count
        product.ViewCount++;
        await _context.SaveChangesAsync(cancellationToken);

        var productDto = new ProductDetailDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            DetailedDescription = product.DetailedDescription,
            Price = product.Price,
            CompareAtPrice = product.CompareAtPrice,
            CategoryName = product.Category.Name,
            Brand = product.Brand,
            SKU = product.SKU,
            StockQuantity = product.StockQuantity,
            ViewCount = product.ViewCount,
            IsFeatured = product.IsFeatured,
            AverageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : 0,
            ReviewCount = product.Reviews.Count,
            Images = product.Images.OrderBy(i => i.DisplayOrder).Select(i => i.Url).ToList(),
            Variants = product.Variants.Where(v => v.IsActive).Select(v => new ProductVariantDto
            {
                Id = v.Id,
                Size = v.Size,
                Color = v.Color,
                ColorHex = v.ColorHex,
                IsInStock = v.StockQuantity > 0,
                AdditionalPrice = v.AdditionalPrice
            }).ToList()
        };

        return Result<ProductDetailDto>.Success(productDto);
    }
}

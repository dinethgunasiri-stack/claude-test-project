using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PaginatedList<ProductDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PaginatedList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Variants)
            .Include(p => p.Images)
            .Include(p => p.Reviews)
            .Where(p => p.IsActive)
            .AsQueryable();

        // Apply filters
        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= request.MaxPrice.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                p.Description.ToLower().Contains(searchTerm) ||
                p.Brand.ToLower().Contains(searchTerm));
        }

        if (request.IsFeatured.HasValue)
        {
            query = query.Where(p => p.IsFeatured == request.IsFeatured.Value);
        }

        // Apply sorting
        query = request.SortBy.ToLower() switch
        {
            "price" => request.SortOrder == "desc"
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),
            "date" => request.SortOrder == "desc"
                ? query.OrderByDescending(p => p.CreatedDate)
                : query.OrderBy(p => p.CreatedDate),
            _ => query.OrderBy(p => p.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CompareAtPrice = p.CompareAtPrice,
                CategoryName = p.Category.Name,
                Brand = p.Brand,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count,
                IsFeatured = p.IsFeatured,
                Images = p.Images.OrderBy(i => i.DisplayOrder).Select(i => i.Url).ToList(),
                Variants = p.Variants.Where(v => v.IsActive).Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Size = v.Size,
                    Color = v.Color,
                    ColorHex = v.ColorHex,
                    IsInStock = v.StockQuantity > 0,
                    AdditionalPrice = v.AdditionalPrice
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        var paginatedList = PaginatedList<ProductDto>.Create(products, totalCount, request.PageNumber, request.PageSize);
        return Result<PaginatedList<ProductDto>>.Success(paginatedList);
    }
}

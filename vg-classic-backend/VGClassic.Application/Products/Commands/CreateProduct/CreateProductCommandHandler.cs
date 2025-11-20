using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;
using VGClassic.Domain.Entities;

namespace VGClassic.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Validate category exists
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId && c.IsActive, cancellationToken);

        if (!categoryExists)
        {
            return Result<int>.Failure("Category not found");
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            DetailedDescription = request.DetailedDescription,
            Price = request.Price,
            CompareAtPrice = request.CompareAtPrice,
            CategoryId = request.CategoryId,
            Brand = request.Brand,
            SKU = request.SKU,
            StockQuantity = request.StockQuantity,
            IsFeatured = request.IsFeatured,
            IsActive = true,
            PublishedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId
        };

        // Add images
        for (int i = 0; i < request.ImageUrls.Count; i++)
        {
            product.Images.Add(new ProductImage
            {
                Url = request.ImageUrls[i],
                Alt = request.Name,
                DisplayOrder = i,
                IsPrimary = i == 0,
                CreatedDate = DateTime.UtcNow
            });
        }

        // Add variants
        foreach (var variantDto in request.Variants)
        {
            product.Variants.Add(new ProductVariant
            {
                Size = variantDto.Size,
                Color = variantDto.Color,
                ColorHex = variantDto.ColorHex,
                SKU = variantDto.SKU,
                AdditionalPrice = variantDto.AdditionalPrice,
                StockQuantity = variantDto.StockQuantity,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            });
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(product.Id);
    }
}

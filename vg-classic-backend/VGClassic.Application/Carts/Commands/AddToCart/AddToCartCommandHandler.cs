using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Carts.Queries.GetCart;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;
using VGClassic.Domain.Entities;

namespace VGClassic.Application.Carts.Commands.AddToCart;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<CartDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddToCartCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<CartDto>.Failure("User not authenticated");
        }

        // Get or create cart
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
        }

        // Validate product
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive, cancellationToken);

        if (product == null)
        {
            return Result<CartDto>.Failure("Product not found");
        }

        if (product.StockQuantity < request.Quantity)
        {
            return Result<CartDto>.Failure("Insufficient stock");
        }

        // Check if item already exists
        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == request.ProductId && i.VariantId == request.VariantId);

        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
            existingItem.UpdatedDate = DateTime.UtcNow;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = request.ProductId,
                VariantId = request.VariantId,
                Quantity = request.Quantity,
                Price = product.Price,
                CreatedDate = DateTime.UtcNow
            });
        }

        cart.LastActivityDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        var cartDto = new CartDto
        {
            Id = cart.Id,
            Items = cart.Items.Select(i => new CartItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                ImageUrl = i.Product.Images.OrderBy(img => img.DisplayOrder).FirstOrDefault()?.Url,
                Quantity = i.Quantity,
                Price = i.Price,
                VariantSize = i.Variant?.Size,
                VariantColor = i.Variant?.Color
            }).ToList()
        };

        return Result<CartDto>.Success(cartDto);
    }
}

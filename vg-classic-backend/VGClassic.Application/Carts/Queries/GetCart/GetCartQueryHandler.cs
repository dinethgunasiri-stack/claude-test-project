using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Carts.Queries.GetCart;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetCartQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<CartDto>.Failure("User not authenticated");
        }

        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                    .ThenInclude(p => p.Images)
            .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cart == null)
        {
            return Result<CartDto>.Success(new CartDto());
        }

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

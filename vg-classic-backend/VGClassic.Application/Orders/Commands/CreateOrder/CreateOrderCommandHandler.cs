using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;
using VGClassic.Domain.Entities;
using VGClassic.Domain.Enums;

namespace VGClassic.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<int>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<int>.Failure("User not authenticated");
        }

        // Get cart with items
        var cart = await _context.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .Include(c => c.Items)
                .ThenInclude(i => i.Variant)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cart == null || !cart.Items.Any())
        {
            return Result<int>.Failure("Cart is empty");
        }

        var subtotal = cart.Items.Sum(i => i.Price * i.Quantity);
        var shipping = 10.00m; // Fixed shipping
        var tax = subtotal * 0.08m; // 8% tax
        var total = subtotal + shipping + tax;

        var order = new Order
        {
            OrderNumber = $"VG{DateTime.UtcNow:yyyyMMddHHmmss}",
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            SubtotalAmount = subtotal,
            ShippingAmount = shipping,
            TaxAmount = tax,
            DiscountAmount = 0,
            TotalAmount = total,
            ShippingFirstName = request.ShippingFirstName,
            ShippingLastName = request.ShippingLastName,
            ShippingAddressLine1 = request.ShippingAddressLine1,
            ShippingAddressLine2 = request.ShippingAddressLine2,
            ShippingCity = request.ShippingCity,
            ShippingState = request.ShippingState,
            ShippingZipCode = request.ShippingZipCode,
            ShippingCountry = request.ShippingCountry,
            ShippingPhone = request.ShippingPhone,
            CustomerNotes = request.CustomerNotes,
            CreatedDate = DateTime.UtcNow
        };

        // Add order items
        foreach (var cartItem in cart.Items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = cartItem.ProductId,
                VariantId = cartItem.VariantId,
                ProductName = cartItem.Product.Name,
                VariantSize = cartItem.Variant?.Size,
                VariantColor = cartItem.Variant?.Color,
                UnitPrice = cartItem.Price,
                Quantity = cartItem.Quantity,
                Discount = 0,
                TotalPrice = cartItem.Price * cartItem.Quantity,
                CreatedDate = DateTime.UtcNow
            });
        }

        _context.Orders.Add(order);

        // Clear cart
        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(order.Id);
    }
}

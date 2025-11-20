using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VGClassic.Application.Common.Interfaces;
using VGClassic.Application.Common.Models;

namespace VGClassic.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (product == null)
        {
            return Result.Failure("Product not found");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.DetailedDescription = request.DetailedDescription;
        product.Price = request.Price;
        product.CompareAtPrice = request.CompareAtPrice;
        product.CategoryId = request.CategoryId;
        product.Brand = request.Brand;
        product.StockQuantity = request.StockQuantity;
        product.IsFeatured = request.IsFeatured;
        product.IsActive = request.IsActive;
        product.UpdatedDate = DateTime.UtcNow;
        product.UpdatedBy = _currentUserService.UserId;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

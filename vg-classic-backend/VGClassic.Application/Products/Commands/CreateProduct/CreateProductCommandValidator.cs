using System;
using System.Threading;
using FluentValidation;

namespace VGClassic.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid category must be selected");

        RuleFor(x => x.SKU)
            .NotEmpty().WithMessage("SKU is required");

        RuleFor(x => x.Variants)
            .NotEmpty().WithMessage("At least one product variant is required");

        RuleFor(x => x.ImageUrls)
            .NotEmpty().WithMessage("At least one image is required");
    }
}
